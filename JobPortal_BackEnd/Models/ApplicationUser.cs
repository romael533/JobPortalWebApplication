using Microsoft.AspNetCore.Identity;


namespace JobPortal_BackEnd.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; } = false;
}
