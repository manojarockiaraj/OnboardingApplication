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
}
