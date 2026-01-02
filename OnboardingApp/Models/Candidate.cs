using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingApp.Models;

public class Candidate
{
    [Key]
    [Column("candidateid")]
    public int Id { get; set; }

    [Column("employeeid")]
    [Required(ErrorMessage = "Employee Id is Mandatory")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Employee Id must contain only numbers")]
    public int EmployeeId { get; set; }

    [Column("fullname")]
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("grade")]
    [Required(ErrorMessage = "Grade is required")]
    [StringLength(50)]
    public string? Grade { get; set; }

    [StringLength(500, ErrorMessage = "Skills cannot exceed 500 characters")]
    [Column("skillset")]
    [Required(ErrorMessage = "Skills is required")]
    [Display(Name = "Skills (comma-separated)")]
    public string? Skills { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    [Column("contactemail")]
    [StringLength(150)]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    [Required(ErrorMessage = "Phone is required")]
    [Column("contactphone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("residence")]
    [Required(ErrorMessage = "Location is required")]
    [StringLength(150)]
    public string? Location { get; set; }

    [Column("availableforinterview")]
    [Required(ErrorMessage = "Please select whether available for interview")]
    public bool? AvailableForInterview { get; set; }

    [StringLength(2000, ErrorMessage = "Summary is too long")]
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
    [Column("isactive")]
    public bool IsActive { get; set; } = true;

    [NotMapped]
    public JobPosting? JobPostingDetails { get; set; }

    public List<CandidateStatusEvaluation>? CandidateStatusEvaluations { get; set; }
}