using Microsoft.AspNetCore.Identity.UI.Services;

namespace AlDawarat_W_AlEngazat.Models {
    public class EmailSender : IEmailSender {
        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            // logic for Sending Emails in Future
            return Task.CompletedTask;
        }
    }
}
