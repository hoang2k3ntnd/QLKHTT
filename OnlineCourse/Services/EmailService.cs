// OnlineCourse/Services/EmailService.cs
using OnlineCourse.Interfaces; // Thêm namespace của giao diện
using System.Net;
using System.Net.Mail;

namespace OnlineCourse.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;

        public EmailService(IConfiguration config)
        {
            // Lấy config từ appsettings.json
            _smtpHost = config["Email:SmtpHost"]!;
            _smtpPort = int.Parse(config["Email:SmtpPort"]!);
            _smtpUser = config["Email:SmtpUser"]!;
            _smtpPass = config["Email:SmtpPass"]!;
            _fromEmail = config["Email:FromEmail"]!;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}