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

    public DbSet<Candidate> Candidates { get; set; } = null!;

    public DbSet<JobApplication> JobApplications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>()
            .HasIndex(c => c.EmployerId)
            .IsUnique();

        builder.Entity<Job>()
            .HasIndex(j => new { j.JobTitle, j.CompanyId })
            .IsUnique();

        builder.Entity<Candidate>()
            .HasIndex(c => c.CandidateId)
            .IsUnique();

        builder.Entity<JobApplication>()
            .HasIndex(j => new { j.CandidateId, j.JobId })
            .IsUnique();
    }
}

