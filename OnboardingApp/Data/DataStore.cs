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
        new Candidate { Id = 1, Name = "Alice Johnson", Skills = "C#, SQL, Azure", Email = "alice@example.com", Phone = "555-0100", Location = "London", Summary = "Experienced backend engineer", ResumeFilterStatus = ResumeStatus.Submitted , BpssStatus = SecurityCheckStatus.NotInitiated, FinalStatus = FinalStatus.InProgress,
        Level1Status = CtsInternalInterviewStatus.NotScheduled, Level2Status = ClientInterviewStatus.AwaitingSchedule},
        new Candidate { Id = 2, Name = "Bob Smith", Skills = "JavaScript, React, CSS", Email = "bob@example.com", Phone = "555-0123", Location = "Coventry", Summary = "Frontend specialist", ResumeFilterStatus = ResumeStatus.Submitted, Level1Status = CtsInternalInterviewStatus.NotScheduled },
        new Candidate { Id = 3, Name = "Carol Lee", Skills = "C#, ASP.NET Core, JavaScript", Email = "carol@example.com", Phone = "555-0155", Location = "Coventry", Summary = "Full-stack developer" }
    };

    // Registered users (passwords stored as hashes)
    public static readonly List<User> Users = new List<User>();
}
