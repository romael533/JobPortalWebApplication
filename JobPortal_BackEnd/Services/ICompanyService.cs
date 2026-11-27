using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Controllers;
using JobPortal_BackEnd.Models;


namespace JobPortal_BackEnd.Services;


public interface ICompanyService
{
    Task<string> AddCompany(CompanyRequest request, string employeeId);
    Task<Company?> GetCompanyByEmployeeId(string employeeId);

}
