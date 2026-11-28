namespace JobPortal_BackEnd.Models;

public enum JobApplicationStatus
{
    Applied,
    UnderReview,
    Shortlisted,
    Hired,
    Rejected,
    Withdrawn
}
public class JobApplication
{
	public string Id { get; set; }
	public string CandidateId { get; set; }
	public string JobId { get; set; }
	public JobApplicationStatus Status { get; set; }
	public DateTime AppliedOn { get; set; }
}
