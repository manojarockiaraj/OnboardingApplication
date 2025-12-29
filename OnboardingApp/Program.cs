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

//var conn = "Host=db.daegresabpkwyjrpkpjo.supabase.co;Database=postgres;Username=postgres;Port=5432;Password=Welcome@12dec25;SSL Mode=Require;Trust Server Certificate=true";
var conn = builder.Configuration.GetConnectionString("dbConn");

// var host = "db.daegresabpkwyjrpkpjo.supabase.co";
// var db = "postgres";
// var user = "postgres";
// var password = "Welcome@12dec25";
// var port = "5432";

//var conn = $"Host={host};Port={port};Database={db};Username={user};Password={password};SSL Mode=Prefer;Trust Server Certificate=True;Timeout=15;Command Timeout=30";

if (!string.IsNullOrEmpty(conn))
{
    // Optional: test connection immediately
    try
    {
        //using var testConn = new Npgsql.NpgsqlConnection(conn);
        //testConn.Open();
        Console.WriteLine("Database connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database connection failed: " + ex.ToString());
        throw;
    }

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(conn, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure();
            //npgsqlOptions.Pooling = false; 
        })
    );
}

// Register repositories
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
 
builder.Services.AddScoped<IOnBoardingRepository, OnBoardingRepository>();
 

// Add authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/JobPosting", async (AppDbContext db) =>
{
    return await db.JobPostings.AsNoTracking().Take(10).ToListAsync();
});

app.MapGet("/OnBoardingList", async (IOnBoardingRepository repo) =>
{
    return await repo.GetAllAsync();
});

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

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();