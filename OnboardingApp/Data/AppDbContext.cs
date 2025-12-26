using Microsoft.EntityFrameworkCore;
using OnboardingApp.Models;

namespace OnboardingApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JobPosting> JobPostings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobPosting>(entity =>
        {
            entity.ToTable("public.jobposting");
            entity.HasKey(e => e.Id).HasName("jobposting_pkey");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.ClientName).HasColumnName("clientname");
            entity.Property(e => e.ClientContact).HasColumnName("clientcontact");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.ClientContract).HasColumnName("clientcontracttype");
            entity.Property(e => e.WorkModel).HasColumnName("workmodel");
            entity.Property(e => e.StartDate).HasColumnName("startdate");
            entity.Property(e => e.RequirementFulfilled).HasColumnName("requirementfulfilled");
            entity.Property(e => e.ClientEvaluation).HasColumnName("clientevaluationrequired");
            entity.Property(e => e.Description).HasColumnName("description");
        });
    }
}
