using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/jobs")]
[Authorize(Roles = "Candidate,Admin")]
public class AllJobController : ControllerBase
{
    private readonly IJobService _service;

    public AllJobController(IJobService service)
    {
        _service = service;
    }

    // Candidates see only open jobs; admins see all jobs.
    private bool OpenOnly => !User.IsInRole("Admin");

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        try
        {
            var jobs = await _service.GetAllJobs(OpenOnly);
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllJobById(string id)
    {
        try
        {
            var job = await _service.GetAllJobById(id, OpenOnly);
            return Ok(job);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }

    [HttpPut("{id}/close")]
    public async Task<IActionResult> CloseAnyJob(string id)
    {
        try
        {
            var jobId = await _service.CloseAnyJob(id);
            return Ok(new { message = "Job closed successfully.", jobId = jobId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchJob(string searchTerm, int page=1, int pageSize=10)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))            
                return BadRequest(new { message = "Search term cannot be empty." });            
            if (page < 1)
                return BadRequest(new { message = "Page must be 1 or greater." });
            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new { message = "Page size must be between 1 and 50." });
            var result = await _service.SearchJob(searchTerm.Trim(), page, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
        }
    }
}
