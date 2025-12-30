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


    [NotMapped]
    [Display(Name = "Cognizant Internal Interviewer Name")]
    public string CTSInternalInterviewerName { get; set; } = string.Empty;

    [NotMapped]
    [Display(Name = "Client Interviewer Name")]
    public string ClientInterviewerName { get; set; } = string.Empty;


    [NotMapped]
    [Display(Name = "Resume Filter")]
    public ResumeStatus ResumeFilterStatus { get; set; } = ResumeStatus.Submitted;

    [NotMapped]
    [Display(Name = "Cognizant Interview")]
    public CtsInternalInterviewStatus Level1Status { get; set; } = CtsInternalInterviewStatus.NA;

    [NotMapped]
    [Display(Name = "Client Interview")]
    public ClientInterviewStatus Level2Status { get; set; } = ClientInterviewStatus.NA;

    [NotMapped]
    [Display(Name = "Final Status")]
    public FinalStatus FinalStatus { get; set; } = FinalStatus.NA;

    [NotMapped]
    [Display(Name = "BPSS Status")]
    public SecurityCheckStatus BpssStatus { get; set; } = SecurityCheckStatus.NA;

    [NotMapped]
    public bool IsSelected { get; set; } = false;

    [NotMapped]
    public JobPosting? JobPostingDetails { get; set; }

    public List<CandidateStatusEvaluation>? CandidateStatusEvaluations { get; set; }
}
        