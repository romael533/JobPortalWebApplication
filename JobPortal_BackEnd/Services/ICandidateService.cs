using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Controllers;


namespace JobPortal_BackEnd.Services;


public interface ICandidateService
{
	public Task<string> AddProfile(CandidateRequest reuqest, string candidateId);
	public Task<string> UpdateProfile(CandidateRequest reuqest, string candidateId);
	public Task<Candidate?> GetProfile(string candidateId);
	public Task<string> ApplyJob(string candidateId, string jobId);
}
