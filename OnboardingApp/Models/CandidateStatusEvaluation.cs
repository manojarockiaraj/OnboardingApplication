using System;
namespace OnboardingApp.Models
{
	public class CandidateStatusEvaluation
	{
        public int EvaluationId { get; set; }

        public int CandidateId { get; set; }

        public int JobPostingId { get; set; }

        public ResumeStatus ResumeStatus { get; set; } = ResumeStatus.Submitted;

        public string? ResumeReviewedBy { get; set; }

        public DateTime? ResumeReviewedDate { get; set; }

        public CtsInternalInterviewStatus CtsInternalInterviewStatus { get; set; } = CtsInternalInterviewStatus.NA;

        public string? CtsInternalInterviewerName { get; set; }

        public DateTime? CtsInternalInterviewDate { get; set; }

        public string? CtsInternalFeedback { get; set; }

        public ClientInterviewStatus ClientInterviewStatus { get; set; } = ClientInterviewStatus.NA;

        public string? ClientInterviewerName { get; set; }

        public DateTime? ClientInterviewDate { get; set; }

        public string? ClientFeedback { get; set; }

        public FinalStatus FinalStatus { get; set; } = FinalStatus.NA;

        public DateTime? FinalDecisionDate { get; set; }

        public SecurityCheckStatus SecurityCheckStatus { get; set; } = SecurityCheckStatus.NA;

        public DateTime? SecurityCheckCompletedDate { get; set; }

        public bool? OnboardingRequestSent { get; set; } = false;

        public DateTime? OnboardingRequestDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}

