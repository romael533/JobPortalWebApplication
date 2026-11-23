using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("/api/auth/register")]
public class RegisterUserController : ControllerBase
{
    private readonly IRegisterUserService _service;

    public RegisterUserController(IRegisterUserService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] RegisterRequest request)
    {
        var userid = await _service.RegisterUserAsync(request);
        return Ok(new { UserId = userid });
    }
}

