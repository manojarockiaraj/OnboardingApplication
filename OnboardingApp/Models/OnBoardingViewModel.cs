using System;
namespace OnboardingApp.Models
{
	public class OnBoardingViewModel
	{
        public Candidate? Candidate { get; set; }
        public OnBoardingList OnBoardingItems { get; set; }
        public int? CandidateId { get; set; }
    }
}

