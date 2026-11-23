using Microsoft.AspNetCore.Identity;
using JobPortal_BackEnd.Models;

namespace JobPortal_BackEnd.Services;

public class RegisterUserService : IRegisterUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> RegisterUserAsync(RegisterRequest request)
    {
        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, request.Role.ToString());
        return user.Id;
    }
}