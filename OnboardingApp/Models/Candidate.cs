using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingApp.Models;

public class Candidate
{
    [Key]
    [Column("candidateid")]
    public int Id { get; set; }

    [Column("fullname")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("skillset")]
    [StringLength(500)]
    [Display(Name = "Skills (comma-separated)")]
    public string? Skills { get; set; }

    [Column("contactemail")]
    [StringLength(150)]
    [EmailAddress]
    public string? Email { get; set; }

    [Column("contactphone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("residence")]
    [StringLength(150)]
    public string? Location { get; set; }

    [Column("availableforinterview")]
    public bool? AvailableForInterview { get; set; }

    [Column("profilesummary")]
    public string? Summary { get; set; }

    // Interviewer names
    [NotMapped]
    [Display(Name = "Cognizant Internal Interviewer Name")]
    public string CTSInternalInterviewerName { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Client Interviewer Name")]
    public string ClientInterviewerName { get; set; } = string.Empty;

    // Status fields
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
}
