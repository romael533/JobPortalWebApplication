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
        var company = await _db.Companies.FirstOrDefaultAsync(c => c.EmployerId == employeeId);
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
            JobType = request.JobType!.Value,
            JobSkills = request.JobSkills,
            ExperienceSummary = request.ExperienceSummary,
            EmployerId = employeeId,
            JobStatus = JobStatus.Open,
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
        return await _db.Jobs.Where(j => j.EmployerId == employeeId).ToListAsync();
    }

    public async Task<Job> GetJobById(string id, string employeeId)
    {
        return await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == employeeId);
    }

    public async Task<string> UpdateJob(string id, JobRequest request, string employeeId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        job.JobTitle = request.JobTitle;
        job.JobDescription = request.JobDescription;
        job.JobLocation = request.JobLocation;
        job.JobType = request.JobType!.Value;
        job.JobSkills = request.JobSkills;
        job.ExperienceSummary = request.ExperienceSummary;
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
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        job.JobStatus = JobStatus.Closed;
        _db.Jobs.Update(job);
        await _db.SaveChangesAsync();        
        return job.Id;
    }

    public async Task DeleteJob(string id, string employeeId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id && j.EmployerId == employeeId);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        _db.Jobs.Remove(job);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Job>> GetAllJobs(bool openOnly)
    {
        var query = _db.Jobs.AsQueryable();
        if (openOnly)
        {
            query = query.Where(j => j.JobStatus == JobStatus.Open);
        }
        return await query.ToListAsync();
    }

    public async Task<Job> GetAllJobById(string id, bool openOnly)
    {
        var query = _db.Jobs.Where(j => j.Id == id);
        if (openOnly)
        {
            query = query.Where(j => j.JobStatus == JobStatus.Open);
        }
        var job = await query.FirstOrDefaultAsync();
        if (job == null)
        {
            throw new KeyNotFoundException("Job not found.");
        }
        return job;
    }

    public async Task<string> CloseAnyJob(string id)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id);
        if (job == null)
        {
            throw new InvalidOperationException("Job not found.");
        }
        job.JobStatus = JobStatus.Closed;
        _db.Jobs.Update(job);
        await _db.SaveChangesAsync();
        return job.Id;
    }

    public async Task<PagedResult<JobSummary>> SearchJob(string searchTerm, int page, int pageSize)
    {
        var pattern = $"%{searchTerm}%";

        var query =
        from j in _db.Jobs
        join c in _db.Companies on j.CompanyId equals c.Id
        where j.JobStatus == JobStatus.Open
       && (EF.Functions.ILike(j.JobTitle, pattern)
        || EF.Functions.ILike(j.JobLocation, pattern)
        || EF.Functions.ILike(j.JobDescription, pattern)
        || EF.Functions.ILike(j.JobSkills, pattern)
        || EF.Functions.ILike(j.ExperienceSummary, pattern))
        select new JobSummary { 
        CompanyName = c.CompanyName,
        JobTitle = j.JobTitle,
        JobLocation = j.JobLocation,
        JobType = j.JobType,
        JobDescription = j.JobDescription,
        JobSkills = j.JobSkills,
        ExperienceSummary = j.ExperienceSummary,
        JobId = j.Id
        };

        var total = await query.CountAsync();

        var items = await query
        .OrderBy(r => r.JobTitle)
        .ThenBy(r => r.JobId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return new PagedResult<JobSummary> {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }
}
