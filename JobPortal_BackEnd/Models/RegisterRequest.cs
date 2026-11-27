namespace JobPortal_BackEnd.Models;

public enum Role
{
    candidate,
    employer
}

public class RegisterRequest
{
	public RegisterRequest()
	{
	}
	public string Email { get; set; }
	public string Password { get; set; }
	public Role Role { get; set; }
}
