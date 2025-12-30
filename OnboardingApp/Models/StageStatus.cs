namespace OnboardingApp.Models;

public enum StageStatus
{
    Unknown,
    Pending,
    Passed,
    Failed
}


public enum ResumeStatus
{
    Submitted,
    UnderReview,
    Shortlisted,
    Rejected,
    OnHold
}


public enum CtsInternalInterviewStatus
{
    NA,
    NotScheduled,
    Scheduled,
    Completed,
    Selected,
    Rejected,
    NotAttended
}


public enum ClientInterviewStatus
{
    NA,
    AwaitingSchedule,
    Scheduled,
    Completed,
    Selected,
    Rejected,
    OnHold,
    FeedbackPending
}

public enum FinalStatus
{
    NA,
    InProgress,
    Joined,
    Dropped
}

public enum SecurityCheckStatus
{
    NA,
    NotInitiated,
    InProgress,
    Cleared,
    Failed
}

public enum OnboardingRequestSent
{
    Yes,
    No
}
