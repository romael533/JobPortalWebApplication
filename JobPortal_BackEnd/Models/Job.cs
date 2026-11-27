namespace JobPortal_BackEnd.Models;

public class Job
{
    public string Id { get; set; }
    public string CompanyId { get; set; }
    public string EmployerId { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public string JobLocation { get; set; }
    public string JobType { get; set; }
    public string JobSkills { get; set; }
    public string ExperienceSummary { get; set; }
    public string JobStatus { get; set; }
}
