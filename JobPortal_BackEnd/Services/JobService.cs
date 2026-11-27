using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Data;

namespace JobPortal_BackEnd.Services;


public class JobService : IJobService
{
	private readonly AppDbContext _db;
	public JobService(AppDbContext db)
	{ 
        _db = db;
	}

	public async Task<string> AddJob(JobRequest request, string employeeId)
    {
        var company = await _db.Companies.FirstOrDefaultAsync(c => c.EmployeeId == employeeId);
        if (company == null)
        {
            throw new CompanyNotFoundException("No company found for the given employee ID.");
        }
        var companyId = company.Id;
        var job = new Job
        {
            Id = Guid.NewGuid().ToString(),
            JobTitle = request.JobTitle,
            JobDescription = request.JobDescription,
            JobLocation = request.JobLocation,
            JobType = request.JobType,
            JobSkills = request.JobSkills,
            ExperienceSummary = request.ExperienceSummary,
            EmployeeId = employeeId,
            JobStatus = "Open",
            CompanyId = companyId
        };
        _db.Jobs.Add(job);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value"))
            {
                throw new InvalidOperationException("A job with this employee ID already exists.");
            }
            else
            {
                throw;
            }
        }
        return job.Id;
    }

    public async Task<IEnumerable<Job>> GetJobsByEmployeeId(string employeeId)
    {
        return await _db.Jobs.Where(j => j.EmployeeId == employeeId).ToListAsync();
    }

    public async Task<Job> GetJobById(string id, string employeeId)
    {
        return await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployeeId == employeeId);
    }

    public async Task<string> UpdateJob(string id, JobRequest request, string employeeId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployeeId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        job.JobTitle = request.JobTitle;
        job.JobDescription = request.JobDescription;
        job.JobLocation = request.JobLocation;
        job.JobType = request.JobType;
        job.JobSkills = request.JobSkills;
        job.ExperienceSummary = request.ExperienceSummary;
        job.JobStatus = request.JobStatus;
        try {
            _db.Jobs.Update(job);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value"))
            {
                throw new InvalidOperationException("A job with this employee ID already exists.");
            }
            else
            {
                throw;
            }
        }
        return job.Id;
    }

    public async Task<string> CloseJob(string id, string employeeId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployeeId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        job.JobStatus = "Closed";
        _db.Jobs.Update(job);
        await _db.SaveChangesAsync();        
        return job.Id;
    }

    public async Task DeleteJob(string id, string employeeId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployeeId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        _db.Jobs.Remove(job);
        await _db.SaveChangesAsync();
    }

}
