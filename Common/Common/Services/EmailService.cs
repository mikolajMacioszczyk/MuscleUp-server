using Common.Interfaces;
using Common.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Common.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSettings> _smtpSettings;
        private readonly IOptions<EmailSettings> _emailSettings;

        public EmailService(
            IOptions<SmtpSettings> smtpSettings, 
            IOptions<EmailSettings> emailSettings)
        {
            _smtpSettings = smtpSettings;
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(
            string subject, 
            string address, 
            string name, 
            string content)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_emailSettings.Value.FromName, _emailSettings.Value.FromAddress));
            mailMessage.To.Add(new MailboxAddress(name, address));
            mailMessage.Subject = subject;

            var body = new BodyBuilder() { HtmlBody = content };
            mailMessage.Body = body.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSettings.Value.Host, _smtpSettings.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpSettings.Value.Username, _smtpSettings.Value.Password);
                await smtpClient.SendAsync(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}