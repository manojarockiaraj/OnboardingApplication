using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace OnboardingApp.Models;

[Table("jobposting")]

public class JobPosting : BaseModel
{
    [PrimaryKey("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Skills (comma-separated)")]
    [JsonPropertyName("skills")]
    public string Skills { get; set; } = string.Empty;

    [Display(Name = "Client Name")]
    [JsonPropertyName("clientname")]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Client Contact")]
    [JsonPropertyName("clientcontact")]
    public string ClientContact { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [JsonPropertyName("startdate")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Client Evaluation")]
    [JsonPropertyName("clientevaluationrequired")]
    public bool ClientEvaluation { get; set; }

    [Display(Name = "Client Contract")]
    [Column("clientcontracttype")]
    public string ClientContract { get; set; } = string.Empty; // e.g. "Multicapability", "ESFA"

    [Display(Name = "Work Model")]
    [JsonPropertyName("workmodel")]
    public string WorkModel { get; set; } = string.Empty; // e.g. "Hybrid", "Remote"

    [Display(Name = "Requirement Fulfilled")]
    [JsonPropertyName("requirementfulfilled")]
    public bool Requirementfulfilled { get; set; } 
}
