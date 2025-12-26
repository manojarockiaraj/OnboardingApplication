using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Data;
using OnboardingApp.Models;

namespace OnboardingApp.Controllers;

public class AccountController : Controller
{
    private static readonly List<(string Username, string Password)> _demoUsers = new()
    {
        ("admin", "password"),
        ("user", "password")
    };

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Invalid login");
            return View();
        }

        // Check registered users
        var passwordHash = ComputeHash(password);
        var user = DataStore.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

        // Fallback to demo users
        if (user == null)
        {
            var demo = _demoUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (demo == default)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
            else
            {
                // create a temporary identity for demo user
                var claimsDemo = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, demo.Username)
                };
                var identityDemo = new ClaimsIdentity(claimsDemo, "CookieAuth");
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identityDemo));

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("DisplayName", user.DisplayName ?? user.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string username, string password, string confirmPassword, string displayName, string email)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Username and password are required");
            return View();
        }

        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match");
            return View();
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
            {
                ModelState.AddModelError(string.Empty, "Invalid email address");
                return View();
            }
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Invalid email address");
            return View();
        }

        if (DataStore.Users.Any(u => u.Username == username) || _demoUsers.Any(u => u.Username == username))
        {
            ModelState.AddModelError(string.Empty, "Username already exists");
            return View();
        }

        var user = new User
        {
            Id = DataStore.Users.Any() ? DataStore.Users.Max(u => u.Id) + 1 : 1,
            Username = username,
            PasswordHash = ComputeHash(password),
            DisplayName = displayName,
            Email = email
        };

        DataStore.Users.Add(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("DisplayName", user.DisplayName ?? user.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login", "Account");
    }

    private static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
