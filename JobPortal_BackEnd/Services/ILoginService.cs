using JobPortal_BackEnd.Models;

public interface ILoginService
{
	Task<string> LoginUserAsync(LoginRequest request);
}
