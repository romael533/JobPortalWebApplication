using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/companies")]
[Authorize(Roles = "Admin")]
public class AllCompanyController : ControllerBase
{
	private readonly ICompanyService _service;
    public AllCompanyController(ICompanyService service)
	{
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        try
        {
            var companies = await _service.GetAllCompanies();
            return Ok(companies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }

}
