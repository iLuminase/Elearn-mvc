using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using EbookMVC.Models;

namespace EbookMVC.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _environment;

        public EmailSender(ILogger<EmailSender> logger, IOptions<EmailSettings> emailSettings, IWebHostEnvironment environment)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
            _environment = environment;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            Console.WriteLine("SENDING MAID PING !");
            try
            {
                // Trong môi trường development, có thể chỉ log thay vì gửi thật
                if (_environment.IsDevelopment())
                {
                    _logger.LogInformation("=== EMAIL WOULD BE SENT ===");
                    _logger.LogInformation("To: {Email}", email);
                    _logger.LogInformation("Subject: {Subject}", subject);
                    _logger.LogInformation("Message: {Message}", htmlMessage);
                    _logger.LogInformation("=== END EMAIL ===");

                    // Kiểm tra xem có cấu hình email hợp lệ không
                    if (!string.IsNullOrEmpty(_emailSettings.Username) &&
                        !_emailSettings.Username.Contains("your-email") &&
                        !string.IsNullOrEmpty(_emailSettings.Password) &&
                        !_emailSettings.Password.Contains("your-app-password"))
                    {
                        _logger.LogInformation("Cấu hình email hợp lệ, đang gửi email thật...");
                        await SendEmailViaSMTP(email, subject, htmlMessage);
                    }
                    else
                    {
                        _logger.LogWarning("Cấu hình email chưa được thiết lập đúng. Chỉ log email.");
                    }
                    return;
                }

                // Trong production, gửi email thật
                await SendEmailViaSMTP(email, subject, htmlMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ATTEMP SENDMING EMAIL ");
                _logger.LogError(ex, "Lỗi khi gửi email đến {Email}", email);
                throw;
            }
        }

        private async Task SendEmailViaSMTP(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);


            Console.WriteLine("=== EMAIL WOULD BE SENT ===");
            Console.WriteLine("FROM : " + _emailSettings.SenderEmail);
            Console.WriteLine("TO : " + email);


            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("ERRỎ SENDING EMAIL : " + ex.Message);
                Console.WriteLine("ERRỎ SATUS : " + ex.StatusCode);

                _logger.LogError(ex, "Lỗi khi gửi email đến {Email}", email);
                throw;
            }

            _logger.LogInformation("Email đã được gửi thành công đến {Email}", email);
        }
    }
}
