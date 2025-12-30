using System;
using OnboardingApp.Models;

namespace OnboardingApp.Helper
{
	    public static class Helper
	    {
            public static StageStatus GetDisplayStatus(this CandidateStatusEvaluation eval)
            {
                if (eval.FinalStatus != FinalStatus.NA) return (StageStatus)eval.FinalStatus;
                if (eval.ClientInterviewStatus != ClientInterviewStatus.AwaitingSchedule) return (StageStatus)eval.ClientInterviewStatus;
                if (eval.CtsInternalInterviewStatus != CtsInternalInterviewStatus.NotScheduled) return (StageStatus)eval.CtsInternalInterviewStatus;
                if (eval.ResumeStatus != ResumeStatus.Submitted) return (StageStatus)eval.ResumeStatus;
                if (eval.SecurityCheckStatus != SecurityCheckStatus.NotInitiated) return (StageStatus)eval.SecurityCheckStatus;

                return StageStatus.Unknown;
            }
        }	
}

