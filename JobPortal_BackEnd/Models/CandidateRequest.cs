using System.ComponentModel.DataAnnotations;


namespace JobPortal_BackEnd.Models;

    public class CandidateRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }

    public string Address { get; set; }
    [Required]
    public string Skills { get; set; }
    [Required]
    public string ExperienceSummary { get; set; }

    public string LanguagesKnown { get; set; }
}