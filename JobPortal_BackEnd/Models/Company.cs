namespace JobPortal_BackEnd.Models;

public class Company
{
    public string Id { get; set; }
    public string CompanyName { get; set; }
    public string CompanyEmail { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyDescription { get; set; }
    public string? CompanyWebsite { get; set; }
    public string EmployeeId { get; set; }
}