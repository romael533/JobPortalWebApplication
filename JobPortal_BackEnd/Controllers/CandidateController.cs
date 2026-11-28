using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/v1/candidates")]
[Authorize(Roles = "Candidate")]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _service;
    public CandidateController(ICandidateService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CandidateRequest request) {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(candidateId))
            return Unauthorized();
        try
        {
            var profileId = await _service.AddProfile(request, candidateId);
            return Ok(new { message = "Candidate Profile added successfully", ProfileId = profileId });
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

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CandidateRequest request)
    {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            var profileId = await _service.UpdateProfile(request, candidateId);
            return Ok(new { message = "Candidate Profile updated successfully", ProfileId = profileId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(candidateId))
            return Unauthorized();

        var profile = await _service.GetProfile(candidateId);
        if (profile == null)
            return NotFound();
        return Ok(profile);
    }

    [HttpPost("jobs/{jobId}/apply")]
    public async Task<IActionResult> ApplyJob(string jobId) {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(candidateId))
            return Unauthorized();

        try
        {
            var applicationId = await _service.ApplyJob(candidateId, jobId);
            return Ok(new { message = "Candidate has applied Job successfully", ApplicationId = applicationId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (JobNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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

    [HttpPost("jobs/{jobId}/withdraw")]
    public async Task<IActionResult> WithdrawJobApplication(string jobId)
    {
        var candidateId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(candidateId))
            return Unauthorized();
        try
        {
            await _service.WithdrawJobApplication(candidateId, jobId);
            return Ok(new { message = "Candidate has withdrawn Job application successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
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
}
