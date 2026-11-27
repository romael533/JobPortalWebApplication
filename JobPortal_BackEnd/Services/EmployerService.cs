using JobPortal_BackEnd.Models;
using Microsoft.AspNetCore.Identity;

namespace JobPortal_BackEnd.Services;



public class EmployerService: IEmployerService
{
	private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICompanyService _companyservice;
    
   public EmployerService(UserManager<ApplicationUser> userManager, ICompanyService companyservice)
    {
        _userManager = userManager;
        _companyservice = companyservice;
    }

    public async Task<IEnumerable<EmployerResponse>> GetEmployerWithCompanies() {
        var employers = await _userManager.GetUsersInRoleAsync("Employer");
        var employerIds = employers.Select(u => u.Id).ToList();

        var companies = await _companyservice.GetCompaniesByEmployerIds(employerIds);
        var companyByEmployee = companies.ToDictionary(c => c.EmployerId);

        return employers.Select(e => new EmployerResponse
        {
            Id = e.Id,
            Email = e.Email,
            Phone = e.PhoneNumber,
            IsActive = e.IsActive,
            Company = companyByEmployee.TryGetValue(e.Id, out var company) ? company : null
        }).ToList();
    }

    public async Task<IdentityResult> SetEmployerStatus(string id, bool isActive)
    {
        var employer = await _userManager.FindByIdAsync(id);
        if(employer == null || !await _userManager.IsInRoleAsync(employer, "Employer"))
            return IdentityResult.Failed(new IdentityError { Description = "Employer not found." });
        employer.IsActive = isActive;
        return await _userManager.UpdateAsync(employer);
    }

}
