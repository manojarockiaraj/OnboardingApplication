using System.ComponentModel.DataAnnotations;

namespace OnboardingApp.Models;

public class JobPosting
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Skills (comma-separated)")]
    public string Skills { get; set; } = string.Empty;

    [Display(Name = "Client Name")]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Client Contact")]
    public string ClientContact { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Client Evaluation")]
    public bool ClientEvaluation { get; set; }

    [Display(Name = "Client Contract")]
    public string ClientContract { get; set; } = string.Empty; // e.g. "Multicapability", "ESFA"

    [Display(Name = "Work Model")]
    public string WorkModel { get; set; } = string.Empty; // e.g. "Hybrid", "Remote"
}
