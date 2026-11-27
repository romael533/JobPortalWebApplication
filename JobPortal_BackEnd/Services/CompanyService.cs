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
            EmployerId = employeeId
        };

        _db.Companies.Add(company);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key value"))
            {
                throw new InvalidOperationException("A company with this employee ID already exists.");
            }
            else
            {
                throw;
            }
        }
        return company.Id;
    }

    public async Task<Company?> GetCompanyByEmployeeId(string employeeId)
    {
        return await _db.Companies.FirstOrDefaultAsync(c => c.EmployerId == employeeId);
    }

    public async Task<IEnumerable<Company>> GetAllCompanies()
    {
        return await _db.Companies.ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetCompaniesByEmployerIds(IEnumerable<string> employerIds)
    {
        return await _db.Companies.Where(c => employerIds.Contains(c.EmployerId)).ToListAsync();
    }
}