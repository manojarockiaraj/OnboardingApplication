using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using OnboardingApp.Services;
using OnboardingApp.Data;
using Microsoft.EntityFrameworkCore;
using OnboardingApp.Repositories;


AppContext.SetSwitch("System.Net.DisableIPv6", true);
AppContext.SetSwitch("System.Net.Sockets.DisableDualMode", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Global authorization policy: require authenticated user for all controllers/actions
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
// Register email service
builder.Services.AddTransient<IEmailService, SmtpEmailService>();

// Register EF Core with Postgres (Supabase)
//var conn = builder.Configuration.GetConnectionString("postgresql://postgres:Welcome@12@db.daegresabpkwyjrpkpjo.supabase.co:5432/postgres");

var conn = "Host=db.daegresabpkwyjrpkpjo.supabase.co;Database=postgres;Username=postgres;Password=Welcome@12dec25;SSL Mode=Require;Trust Server Certificate=true";
if (!string.IsNullOrEmpty(conn))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(conn)
    );
}

// Register repositories
builder.Services.AddScoped<IJobRepository, JobRepository>();

// Add authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
