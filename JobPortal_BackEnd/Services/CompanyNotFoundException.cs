namespace JobPortal_BackEnd.Services;


public class CompanyNotFoundException : Exception
{
	public CompanyNotFoundException(string message) : base(message)
	{
	}
}
