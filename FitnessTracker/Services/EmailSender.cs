using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Fitness_Tracker.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Log the email to the console (for debugging/testing purposes)
            Console.WriteLine($"Sending email to {email} with subject: {subject}");
            Console.WriteLine($"Message: {htmlMessage}");
            return Task.CompletedTask; // Pretend the email was sent successfully
        }
    }
}
