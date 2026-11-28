namespace JobPortal_BackEnd.Models;

public enum JobType
{
    FullTime,
    PartTime,
    Contract,
    Internship
}

public enum JobStatus
{
    Open,
    Closed
}

public class Job
{
    public string Id { get; set; }
    public string CompanyId { get; set; }
    public string EmployerId { get; set; }
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public string JobLocation { get; set; }
    public JobType JobType { get; set; }
    public string JobSkills { get; set; }
    public string ExperienceSummary { get; set; }
    public JobStatus JobStatus { get; set; }
}
