using System;
namespace OnboardingApp.Models
{
	public class OnBoardingList
	{
        public int Id { get; set; }
        public string? title { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }
        public bool isactive { get; set; }

    }
}

