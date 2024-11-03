using Humanizer;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Models.Smtp;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace QueueManagementSystem.MVC.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ILogger<EmailSenderService> _logger;
        private readonly IDbContextFactory<QueueManagementSystemContext> _dbContextFactory;

        public EmailSenderService(ILogger<EmailSenderService> logger, IDbContextFactory<QueueManagementSystemContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
        }
        public async Task SendEmailAsync(EmailMessageModel message)
        {
            try
            {
                // Fetch email configuration from the database
                using var context = await _dbContextFactory.CreateDbContextAsync();
                var emailConfig = await context.SmptSettings.FirstOrDefaultAsync();

                if (emailConfig == null)
                {
                    throw new InvalidOperationException("Email configuration not found in the database.");
                }

                // Create the MimeMessage
                var mimeMessage = new MimeMessage();

                // Add the sender
                mimeMessage.From.Add(new MailboxAddress(emailConfig.SenderName, emailConfig.Sender));

                // Add the recipients
                foreach (var email in message.Recipients!)
                    mimeMessage.To.Add(MailboxAddress.Parse(email));

                // Add the subject
                mimeMessage.Subject = message.Subject;

                var builder = new BodyBuilder { HtmlBody = message.Body };

                //Handle attachments
                if (message.Attachments?.Count > 0)
                    foreach (var attachment in message.Attachments)
                        builder.Attachments.Add(attachment.FileName, attachment.Content);

                // Construct the message body
                mimeMessage.Body = builder.ToMessageBody();

                // Create the Smtp client
                using var client = new SmtpClient();

                await client.ConnectAsync(emailConfig.MailServer, emailConfig.MailPort, MailKit.Security.SecureSocketOptions.Auto);

                //Remove any OAuth functionality as we won't be using it. 
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(emailConfig.Sender, emailConfig.Password);

                // Send the message
                await client.SendAsync(mimeMessage);

                // Disconnect
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {message.Recipients}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending email: {ex.Message}");
                throw;
            }
        }
    }
}
