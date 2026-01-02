using OnboardingApp.Models;
using System.Net.Mail;
using System.Net;

namespace OnboardingApp.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;

    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    public Task SendOnboardingRequestAsync(Candidate candidate)
    {
        // Simple SMTP send using configuration; in real app secure secrets and use proper client
        var smtpHost = _config["Email:SmtpHost"] ?? "localhost";
        var smtpPort = int.TryParse(_config["Email:SmtpPort"], out var p) ? p : 25;
        var from = _config["Email:From"] ?? "noreply@example.com";
        var to = _config["Email:OnboardingRecipient"] ?? "onboarding@example.com";

        var subject = $"Onboarding request: {candidate.Name}";
        var body = $"Please onboard candidate:\n\nName: {candidate.Name}\nEmail: {candidate.Email}\nPhone: {candidate.Phone}\nLocation: {candidate.Location}\nSkills: {candidate.Skills}\nSummary: {candidate.Summary}\n";

        var message = new MailMessage(from, to, subject, body);

        using var client = new SmtpClient(smtpHost, smtpPort);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        // Optional credentials from config
        if (!string.IsNullOrEmpty(_config["Email:SmtpUser"]))
        {
            client.Credentials = new NetworkCredential(_config["Email:SmtpUser"], _config["Email:SmtpPass"]);
        }

        client.EnableSsl = bool.TryParse(_config["Email:EnableSsl"], out var ssl) ? ssl : false;

        return client.SendMailAsync(message);
    }
}
