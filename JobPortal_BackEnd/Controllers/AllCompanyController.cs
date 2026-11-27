using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/all")]
[Authorize(Roles = "Admin")]
public class AllCompanyController : ControllerBase
{
	private readonly ICompanyService _service;
    private readonly IEmployerService _employerservice;
    public AllCompanyController(ICompanyService service, IEmployerService employerservice)
	{
        _service = service;
        _employerservice = employerservice;
    }

    [HttpGet("companies")]
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

    [HttpGet("employers")]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employers = await _employerservice.GetEmployerWithCompanies();
        return Ok(employers);
    }

    [HttpPost("employers/status/update/{id}")]
    public async Task<IActionResult> SetEmployerStatus(string id, bool isActive) {
        try {
            var result = await _employerservice.SetEmployerStatus(id, isActive);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            return Ok(new { message = "Employee status updated", EmployerId = id });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }        
    }
}
