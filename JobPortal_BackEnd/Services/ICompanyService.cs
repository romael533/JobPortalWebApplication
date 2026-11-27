using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Controllers;


namespace JobPortal_BackEnd.Services;


public interface ICompanyService
{
    Task<string> AddCompany(CompanyRequest request, string employeeId);
    Task<Company?> GetCompanyByEmployeeId(string employeeId);
    Task<IEnumerable<Company>> GetAllCompanies();
}
