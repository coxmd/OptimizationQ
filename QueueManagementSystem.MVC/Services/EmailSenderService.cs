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
        private readonly SmtpClient _smtpClient;

        public EmailSenderService(ILogger<EmailSenderService> logger, IDbContextFactory<QueueManagementSystemContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _smtpClient = new SmtpClient();
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

                // Connect the Smtp client if not already connected
                if (!_smtpClient.IsConnected)
                {
                    await _smtpClient.ConnectAsync(emailConfig.MailServer, emailConfig.MailPort, MailKit.Security.SecureSocketOptions.Auto);
                    _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    await _smtpClient.AuthenticateAsync(emailConfig.Sender, emailConfig.Password);
                }

                // Send the message
                await _smtpClient.SendAsync(mimeMessage);

                _logger.LogInformation($"Email sent successfully to {message.Recipients}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending email: {ex.Message}");
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_smtpClient.IsConnected)
            {
                await _smtpClient.DisconnectAsync(true);
            }
            _smtpClient.Dispose();
        }
    }
}
