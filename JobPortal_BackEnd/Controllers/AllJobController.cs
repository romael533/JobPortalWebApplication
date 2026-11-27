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
}
