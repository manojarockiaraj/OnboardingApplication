using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace OnboardingApp.Models;


public class Candidate : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Skills (comma-separated)")]
    public string Skills { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    // Interviewer names
    [Display(Name = "Cognizant Internal Interviewer Name")]
    public string CTSInternalInterviewerName { get; set; } = string.Empty;

    [Display(Name = "Client Interviewer Name")]
    public string ClientInterviewerName { get; set; } = string.Empty;

    // Status fields
    [Display(Name = "Resume Filter")]
    public StageStatus ResumeFilterStatus { get; set; } = StageStatus.Unknown;

    [Display(Name = "Cognizant Interview")]
    public StageStatus Level1Status { get; set; } = StageStatus.Unknown;

    [Display(Name = "Client Interview")]
    public StageStatus Level2Status { get; set; } = StageStatus.Unknown;

    [Display(Name = "Final Status")]
    public StageStatus FinalStatus { get; set; } = StageStatus.Unknown;

    [Display(Name = "BPSS Status")]
    public StageStatus BpssStatus { get; set; } = StageStatus.Unknown;
}
