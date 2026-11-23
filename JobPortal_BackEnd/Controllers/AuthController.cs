using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JobPortal_BackEnd.Models;
using JobPortal_BackEnd.Services;

namespace JobPortal_BackEnd.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokens;

    public AuthController(UserManager<ApplicationUser> userManager, TokenService tokens)
    {
        _userManager = userManager;
        _tokens = tokens;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized();

        if (!user.IsActive)
            return StatusCode(StatusCodes.Status403Forbidden, "User account is not active.");

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new { Token = _tokens.CreateToken(user, roles) });
    }
}
