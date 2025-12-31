using Microsoft.EntityFrameworkCore;
using OnboardingApp.Models;

namespace OnboardingApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JobPosting> JobPostings { get; set; }
 
    public DbSet<OnBoardingList> OnBoardingLists { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<CandidateStatusEvaluation> CandidateStatusEvaluation { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobPosting>(entity =>
        {
            entity.ToTable("jobposting");
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

 
        modelBuilder.Entity<OnBoardingList>(entity =>
        {
            entity.ToTable("onboardinglist");
            entity.HasKey(e => e.Id).HasName("onboardinglist_pkey");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.title).HasColumnName("title");
            entity.Property(e => e.type).HasColumnName("type");
            entity.Property(e => e.description).HasColumnName("description");
            entity.Property(e => e.isactive).HasColumnName("isactive");
        });

        //modelBuilder.Entity<Candidate>(entity =>
        //{
        //    entity.ToTable("candidate");
        //    entity.HasKey(e => e.Id).HasName("candidates_papplicantidkey");
        //    entity.Property(e => e.Id).HasColumnName("applicantid");
        //    entity.Property(e => e.Name).HasColumnName("fullname");
        //    entity.Property(e => e.Skills).HasColumnName("skillset");
        //    entity.Property(e => e.Email).HasColumnName("contactemail");
        //    entity.Property(e => e.Phone).HasColumnName("contactphone");
        //    entity.Property(e => e.Location).HasColumnName("residence");
        //    entity.Property(e => e.AvailableForInterview).HasColumnName("availableforinterview");
        //    entity.Property(e => e.Summary).HasColumnName("profilesummary");
        //    // other properties map to default column names (if present)  
        //}); 

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.ToTable("candidate");
            entity.HasKey(e => e.Id)
          .HasName("candidate_pkey");

    entity.Property(e => e.Id)
          .HasColumnName("candidateid");

            entity.Property(e => e.EmployeeId)
        .HasColumnName("employeeid");

            entity.Property(e => e.Name)
          .HasColumnName("fullname")
          .HasMaxLength(150);

    entity.Property(e => e.Skills)
          .HasColumnName("skillset")
          .HasMaxLength(500);

    entity.Property(e => e.Email)
          .HasColumnName("contactemail")
          .HasMaxLength(150);

    entity.Property(e => e.Phone)
          .HasColumnName("contactphone")
          .HasMaxLength(20);

    entity.Property(e => e.Location)
          .HasColumnName("residence")
          .HasMaxLength(150);

    entity.Property(e => e.AvailableForInterview)
          .HasColumnName("availableforinterview");

            entity.Property(e => e.IsActive)
              .HasColumnName("isactive");

            entity.Property(e => e.Summary)
          .HasColumnName("profilesummary");
            //entity.Property(e => e.CTSInternalInterviewerName).HasColumnName("ctsinternalinterviewername");
            //entity.Property(e => e.ClientInterviewerName).HasColumnName("clientinterviewername");
            //entity.Property(e => e.ResumeFilterStatus).HasColumnName("resumefilterstatus");
            //entity.Property(e => e.Level1Status).HasColumnName("level1status");
            //entity.Property(e => e.Level2Status).HasColumnName("level2status");
            //entity.Property(e => e.FinalStatus).HasColumnName("finalstatus");
            //entity.Property(e => e.BpssStatus).HasColumnName("bpssstatus");
        });

        

        modelBuilder.Entity<CandidateStatusEvaluation>(entity =>
        {
            entity.ToTable("candidatestatusevaluation");

            entity.HasKey(e => e.EvaluationId)
                  .HasName("candidate_evaluation_pkey");

            entity.Property(e => e.EvaluationId).HasColumnName("evaluationid");
            entity.Property(e => e.CandidateId).HasColumnName("candidateid");
            entity.Property(e => e.JobPostingId).HasColumnName("jobpostingid");

            entity.Property(e => e.ResumeStatus).HasColumnName("resumestatus").HasConversion<string>();
            entity.Property(e => e.ResumeReviewedBy).HasColumnName("resumereviewedby").HasMaxLength(100);
            entity.Property(e => e.ResumeReviewedDate).HasColumnName("resumerevieweddate");



            entity.Property(e => e.CtsInternalInterviewStatus).HasColumnName("ctsinternalinterviewstatus").HasConversion<string>();
            entity.Property(e => e.CtsInternalInterviewerName).HasColumnName("ctsinternalinterviewername").HasMaxLength(100);
            entity.Property(e => e.CtsInternalInterviewDate).HasColumnName("ctsinternalinterviewdate");
            entity.Property(e => e.CtsInternalFeedback).HasColumnName("ctsinternalfeedback").HasMaxLength(500);

            entity.Property(e => e.ClientInterviewStatus).HasColumnName("clientinterviewstatus").HasConversion<string>();
            entity.Property(e => e.ClientInterviewerName).HasColumnName("clientinterviewername").HasMaxLength(100);
            entity.Property(e => e.ClientInterviewDate).HasColumnName("clientinterviewdate");
            entity.Property(e => e.ClientFeedback).HasColumnName("clientfeedback").HasMaxLength(500);

            entity.Property(e => e.FinalStatus).HasColumnName("finalstatus").HasConversion<string>();
            entity.Property(e => e.FinalDecisionDate).HasColumnName("finaldecisiondate");

            entity.Property(e => e.SecurityCheckStatus).HasColumnName("securitycheckstatus").HasConversion<string>();
            entity.Property(e => e.SecurityCheckCompletedDate).HasColumnName("securitycheckcompleteddate");

            entity.Property(e => e.OnboardingRequestSent).HasColumnName("onboardingrequestsent").HasDefaultValue(false);
            entity.Property(e => e.OnboardingRequestDate).HasColumnName("onboardingrequestdate");

            entity.Property(e => e.CreatedDate).HasColumnName("createddate").HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedDate).HasColumnName("updateddate");
        });




    }
}
