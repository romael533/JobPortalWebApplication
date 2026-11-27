using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/companies/mine")]
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
        catch (CompanyNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        var company = await _service.GetCompanyByEmployeeId(employeeId);
        if (company == null)
            return NotFound();
        return Ok(company);
    }
}
