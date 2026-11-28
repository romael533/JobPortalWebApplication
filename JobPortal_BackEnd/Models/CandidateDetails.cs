namespace JobPortal_BackEnd.Models;


public class CandidateDetails
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Skills { get; set; }
    public string ExperienceSummary { get; set; }
    public string LanguagesKnown { get; set; }
    public string CandidateId { get; set; }
    public string ApplicationId { get; set; }
    public DateTime AppliedDate { get; set; }
    public JobApplicationStatus ApplicationStatus { get; set; }
}
