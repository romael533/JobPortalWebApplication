namespace JobPortal_BackEnd.Services;

public class JobNotFoundException : Exception
{
	public JobNotFoundException(string message) : base(message)
    {
	}
}
