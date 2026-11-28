namespace JobPortal_BackEnd.Models;

public class AppliedJobs
{
   public string CompanyName { get; set; }
   public string JobTitle { get; set; }
   public JobApplicationStatus Status { get; set; }
   public DateTime AppliedOn { get; set; }
   public string JobId { get; set; }
   public string CompanyId { get; set; }
   public string ApplicationId { get; set; }
}
