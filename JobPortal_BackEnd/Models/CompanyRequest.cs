using System.ComponentModel.DataAnnotations;

namespace JobPortal_BackEnd.Models;

public class CompanyRequest
{
    [Required]
    public string CompanyName { get; set; }
    [Required]
    public string CompanyEmail { get; set; }

    public string CompanyPhone { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyDescription { get; set; }
    public string CompanyWebsite { get; set; }
}