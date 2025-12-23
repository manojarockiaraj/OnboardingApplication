using OnboardingApp.Models;

namespace OnboardingApp.Services;

public interface IEmailService
{
    Task SendOnboardingRequestAsync(Candidate candidate);
}
