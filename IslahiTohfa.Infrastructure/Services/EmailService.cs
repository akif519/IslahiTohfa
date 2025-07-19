using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var client = new System.Net.Mail.SmtpClient(_settings.SmtpServer, _settings.SmtpPort);
                client.EnableSsl = _settings.EnableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(_settings.Username, _settings.Password);

                using var message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress(_settings.SenderEmail, _settings.SenderName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                await client.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to: {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to: {Email}", to);
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string to, string userName)
        {
            var subject = "مرحباً بك في الإصلاحي تحفة - Welcome to Islahi Tohfa";
            var body = $@"
                <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif;'>
                    <h2>مرحباً {userName}!</h2>
                    <p>أهلاً وسهلاً بك في منصة الإصلاحي تحفة للكتب التعليمية الإسلامية.</p>
                    <p>يمكنك الآن تصفح مكتبتنا الغنية من الكتب الإسلامية وقراءتها مجاناً.</p>
                    <hr>
                    <h3>Welcome {userName}!</h3>
                    <p>Welcome to Islahi Tohfa Islamic Educational Books Platform.</p>
                    <p>You can now browse our rich library of Islamic books and read them for free.</p>
                    <p><a href='https://islahitohfa.com'>زيارة الموقع / Visit Website</a></p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var subject = "إعادة تعيين كلمة المرور - Password Reset";
            var resetUrl = $"https://islahitohfa.com/account/reset-password?token={resetToken}";
            var body = $@"
                <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif;'>
                    <h2>إعادة تعيين كلمة المرور</h2>
                    <p>تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بحسابك.</p>
                    <p><a href='{resetUrl}'>اضغط هنا لإعادة تعيين كلمة المرور</a></p>
                    <p>إذا لم تطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذا البريد الإلكتروني.</p>
                    <hr>
                    <h3>Password Reset</h3>
                    <p>We received a request to reset your account password.</p>
                    <p><a href='{resetUrl}'>Click here to reset your password</a></p>
                    <p>If you didn't request a password reset, please ignore this email.</p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendEmailConfirmationAsync(string to, string confirmationToken)
        {
            var subject = "تأكيد البريد الإلكتروني - Email Confirmation";
            var confirmUrl = $"https://islahitohfa.com/account/confirm-email?token={confirmationToken}";
            var body = $@"
                <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif;'>
                    <h2>تأكيد البريد الإلكتروني</h2>
                    <p>شكراً لك على التسجيل في الإصلاحي تحفة.</p>
                    <p><a href='{confirmUrl}'>اضغط هنا لتأكيد بريدك الإلكتروني</a></p>
                    <hr>
                    <h3>Email Confirmation</h3>
                    <p>Thank you for registering with Islahi Tohfa.</p>
                    <p><a href='{confirmUrl}'>Click here to confirm your email</a></p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }
    }
}
