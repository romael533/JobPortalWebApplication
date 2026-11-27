namespace JobPortal_BackEnd.Models;

public class JobApplication
{
	public string Id { get; set; }
	public string CandidateId { get; set; }
	public string JobId { get; set; }
	public string Status { get; set; }
	public DateTime AppliedOn { get; set; }
}
