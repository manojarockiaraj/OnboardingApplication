using OnboardingApp.Models;

namespace OnboardingApp.Data;

public static class DataStore
{
    public static readonly List<JobPosting> Jobs = new List<JobPosting>
    {
        new JobPosting { Id = 1, Title = "Software Engineer", Skills = "C#, ASP.NET Core, SQL", ClientName = "DFE", ClientContact = "acme@example.com", Location = "London", Description = "Backend developer for API" },
        new JobPosting { Id = 2, Title = "Frontend Developer", Skills = "JavaScript, React, CSS", ClientName = "DEFRA", ClientContact = "contact@beta.com", Location = "Coventry", Description = "Work on UI components" }
    };

    public static readonly List<Candidate> Candidates = new List<Candidate>
    {
        new Candidate { Id = 1, Name = "Alice Johnson", Skills = "C#, SQL, Azure", Email = "alice@example.com", Phone = "555-0100", Location = "London", Summary = "Experienced backend engineer", ResumeFilterStatus = StageStatus.Passed , BpssStatus = StageStatus.Passed, FinalStatus = StageStatus.Passed,
        Level1Status = StageStatus.Passed, Level2Status = StageStatus.Passed},
        new Candidate { Id = 2, Name = "Bob Smith", Skills = "JavaScript, React, CSS", Email = "bob@example.com", Phone = "555-0123", Location = "Coventry", Summary = "Frontend specialist", ResumeFilterStatus = StageStatus.Passed, Level1Status = StageStatus.Pending },
        new Candidate { Id = 3, Name = "Carol Lee", Skills = "C#, ASP.NET Core, JavaScript", Email = "carol@example.com", Phone = "555-0155", Location = "Coventry", Summary = "Full-stack developer" }
    };
}
