using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/companies/mine")]
[Authorize(Roles="Employer")]
public class CompanyController : ControllerBase
{
	private readonly ICompanyService _service;
	public CompanyController(ICompanyService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] CompanyRequest request)
	{
		var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrEmpty(employeeId))
			return Unauthorized();
		try {
            var companyId = await _service.AddCompany(request, employeeId);
            return Ok(new { CompanyId = companyId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }

    }

}
