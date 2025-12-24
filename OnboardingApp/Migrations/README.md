This project uses EF Core migrations. Run the following commands to create migrations and update the Supabase database:

1. dotnet ef migrations add InitialCreate -p OnboardingApp -s OnboardingApp
2. dotnet ef database update -p OnboardingApp -s OnboardingApp

Ensure the Supabase connection string is set in appsettings.json under ConnectionStrings:Supabase before running.
