using System.ComponentModel.DataAnnotations;

namespace JobPortal_BackEnd.Models;

public class JobRequest
{
    [Required]
	public string JobTitle { get; set; }
    [Required]
    public string JobDescription { get; set; }
    [Required]
    public string JobLocation { get; set; }
    [Required]
    public string JobType { get; set; }
    [Required]
    public string JobSkills { get; set; }
    [Required]
    public string ExperienceSummary { get; set; }
    [Required]
    public string JobStatus { get; set; }
}
