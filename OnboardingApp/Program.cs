using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Supabase;
using OnboardingApp.Services;

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
builder.Services.AddTransient<OnboardingApp.Services.IEmailService, OnboardingApp.Services.SmtpEmailService>();

// Add authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

// Configure Supabase client
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseKey = builder.Configuration["Supabase:Key"] ?? Environment.GetEnvironmentVariable("SUPABASE_KEY");

if (!string.IsNullOrWhiteSpace(supabaseUrl) && !string.IsNullOrWhiteSpace(supabaseKey))
{
    var options = new SupabaseOptions
    {
        AutoConnectRealtime = true
    };

    var supabase = new Client(supabaseUrl, supabaseKey, options);
    // Initialize will perform the initial connection; await on top-level is allowed
    await supabase.InitializeAsync();

    // Register supabase client and a typed service to fetch data
    builder.Services.AddSingleton(supabase);
    builder.Services.AddSingleton<ISupabaseService, SupabaseService>();
}
else
{
    // If Supabase isn't configured we still register a no-op implementation so controllers won't fail
    builder.Services.AddSingleton<ISupabaseService, NullSupabaseService>();
}

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
