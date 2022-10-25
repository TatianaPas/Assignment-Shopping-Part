using Microsoft.AspNetCore.Identity.UI.Services;

namespace Assignment.Utility
{
    public class EmailSender : IEmailSender
    {
        //walk around email sender with custom identity
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}