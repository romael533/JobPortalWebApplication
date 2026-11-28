using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Data;

namespace JobPortal_BackEnd.Services;

public class CandidateService : ICandidateService
{
	private readonly AppDbContext _db;
    public CandidateService(AppDbContext db)
	{
		_db = db;
	}

	public async Task<string> AddProfile(CandidateRequest request, string candidateId) { 

		var candidate = new Candidate {
          Id = Guid.NewGuid().ToString(),
          Name = request.Name,
          Email = request.Email,
          Phone = request.Phone,
          Address = request.Address,
          Skills = request.Skills,
          ExperienceSummary = request.ExperienceSummary,
          LanguagesKnown = request.LanguagesKnown,
          CandidateId = candidateId
		};
        _db.Candidates.Add(candidate);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value"))
            {
                throw new InvalidOperationException("A profile already exists for this candidate.");
            }
            else
            {
                throw;
            }
        }
        return candidate.Id;
	}

    public async Task<string> UpdateProfile(CandidateRequest request, string candidateId) {
        var candidate = await _db.Candidates.FirstOrDefaultAsync(j => j.CandidateId == candidateId);
        if (candidate == null)
            throw new KeyNotFoundException("Candidate profile not found.");
        candidate.Name = request.Name;
        candidate.Email = request.Email;
        candidate.Phone = request.Phone;
        candidate.Address = request.Address;
        candidate.Skills = request.Skills;
        candidate.ExperienceSummary = request.ExperienceSummary;
        candidate.LanguagesKnown = request.LanguagesKnown;
        _db.Candidates.Update(candidate);
        await _db.SaveChangesAsync();
        
        return candidate.Id;
    }

    public async Task<Candidate?> GetProfile(string candidateId)
    {
        return await _db.Candidates.FirstOrDefaultAsync(c => c.CandidateId == candidateId);
    }

    public async Task<string> ApplyJob(string candidateId, string jobId) {
        
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
        if (job == null)
        {
            throw new JobNotFoundException("Job not found.");
        }
        if (job.JobStatus == JobStatus.Closed)
        {
            throw new InvalidOperationException("Job already closed.");
        }
        if (!await _db.Candidates.AnyAsync(c => c.CandidateId == candidateId))
            throw new KeyNotFoundException("Create a candidate profile before applying.");

        var jobApplication = new JobApplication {
            Id = Guid.NewGuid().ToString(),
            CandidateId = candidateId,
            JobId = jobId,
            Status = JobApplicationStatus.Applied,
            AppliedOn = DateTime.UtcNow
            };
        _db.JobApplications.Add(jobApplication);
        try {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value"))
            {
                throw new InvalidOperationException("Job already applied by same candidate.");
            }
            else {
                throw;
             }
        }
        return jobApplication.Id;
    }
}
