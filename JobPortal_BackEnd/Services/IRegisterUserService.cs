using JobPortal_BackEnd.Models;

namespace JobPortal_BackEnd.Services;


public interface IRegisterUserService
{
	Task<string> RegisterUserAsync(RegisterRequest request);
}

