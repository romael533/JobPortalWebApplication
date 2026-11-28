using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/jobs/mine")]
[Authorize(Roles="Employer")]
public class JobController : ControllerBase
{
	private readonly IJobService _service;
    public JobController(IJobService service)
	{
		_service = service;
    }

	[HttpPost]
    public async Task<IActionResult> Post([FromBody] JobRequest request)
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        try
        {
            var jobId = await _service.AddJob(request, employeeId);
            return Ok(new { message = "Job created successfully.", JobId = jobId });
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
        var jobs = await _service.GetJobsByEmployeeId(employeeId);
        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        var job = await _service.GetJobById(id, employeeId);
        if (job == null)
            return NotFound();
        return Ok(job);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] JobRequest request)
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        try
        {
            await _service.UpdateJob(id, request, employeeId);
            return Ok(new { message = "Job updated successfully.", JobID = id });
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

    [HttpPut("{id}/close")]
    public async Task<IActionResult> Close(string id)
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        try
        {
            await _service.CloseJob(id, employeeId);
            return Ok(new { message = "Job closed successfully.", JobID = id });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(employeeId))
            return Unauthorized();
        try
        {
            await _service.DeleteJob(id, employeeId);
            return Ok(new { message = "Job deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }    
}
