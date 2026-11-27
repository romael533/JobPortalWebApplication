using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Controllers;

namespace JobPortal_BackEnd.Services;

public interface IJobService
{
    Task<string> AddJob(JobRequest request, string employeeId);
    Task<IEnumerable<Job>> GetJobsByEmployeeId(string employeeId);
    Task<Job> GetJobById(string id, string employeeId);
    Task<string> UpdateJob(string id, JobRequest request, string employeeId);
    Task<string> CloseJob(string id, string employeeId);
    Task DeleteJob(string id, string employeeId);
}
