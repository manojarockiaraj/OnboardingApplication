using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingApp.Models;

public class Candidate
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Skills (comma-separated)")]
    public string Skills { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    // Interviewer names (not stored in DB table)
    [NotMapped]
    [Display(Name = "Cognizant Internal Interviewer Name")]
    public string CTSInternalInterviewerName { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Client Interviewer Name")]
    public string ClientInterviewerName { get; set; } = string.Empty;

    // Status fields (not mapped to DB columns in current schema)
    [NotMapped]
    [Display(Name = "Resume Filter")]
    public StageStatus ResumeFilterStatus { get; set; } = StageStatus.Unknown;

    [NotMapped]
    [Display(Name = "Cognizant Interview")]
    public StageStatus Level1Status { get; set; } = StageStatus.Unknown;

    [NotMapped]
    [Display(Name = "Client Interview")]
    public StageStatus Level2Status { get; set; } = StageStatus.Unknown;

    [NotMapped]
    [Display(Name = "Final Status")]
    public StageStatus FinalStatus { get; set; } = StageStatus.Unknown;

    [NotMapped]
    [Display(Name = "BPSS Status")]
    public StageStatus BpssStatus { get; set; } = StageStatus.Unknown;

    // Optional: map to DB column `availableforinterview` if present
    [Display(Name = "Available for interview")]
    public bool? AvailableForInterview { get; set; }
}
