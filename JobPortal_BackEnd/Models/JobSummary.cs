namespace JobPortal_BackEnd.Models;

public class JobSummary
{
	public string CompanyName { set; get; }
	public string JobTitle { set; get; }
	public string JobLocation { set; get; }
	public JobType JobType { set; get; }
	public string JobDescription { set; get; }
	public string JobSkills { set; get; }
	public string ExperienceSummary { set; get; }
	public string JobId { set; get; }
}