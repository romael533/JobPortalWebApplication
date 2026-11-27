using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using JobPortal_BackEnd.Controllers;


namespace JobPortal_BackEnd.Services;


public interface IEmployerService
{
    Task<IEnumerable<EmployerResponse>> GetEmployerWithCompanies();
    Task<IdentityResult> SetEmployerStatus(string id, bool isActive);
}
