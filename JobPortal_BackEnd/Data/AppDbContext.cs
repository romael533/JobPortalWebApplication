using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobPortal_BackEnd.Models;

namespace JobPortal_BackEnd.Data;


public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;

    public DbSet<Job> Jobs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>()
            .HasIndex(c => c.EmployeeId)
            .IsUnique();

        builder.Entity<Job>()
            .HasIndex(c => new { c.JobTitle, c.CompanyId })
            .IsUnique();
    }
}

