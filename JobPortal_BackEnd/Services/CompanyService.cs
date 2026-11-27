using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Data;

namespace JobPortal_BackEnd.Services;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _db;
    public CompanyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> AddCompany(CompanyRequest request, string employeeId) {

        var company = new Company
        {
            Id = Guid.NewGuid().ToString(),
            CompanyName = request.CompanyName,
            CompanyEmail = request.CompanyEmail,
            CompanyPhone = request.CompanyPhone,
            CompanyAddress = request.CompanyAddress,
            CompanyDescription = request.CompanyDescription,
            CompanyWebsite = request.CompanyWebsite,
            EmployeeId = employeeId
        };

        _db.Companies.Add(company);
        await _db.SaveChangesAsync();

        return company.Id;
    }
}