namespace JobPortal_BackEnd.Models;

public class EmployerResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public Company? Company { get; set; }
}
