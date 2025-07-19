using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendWelcomeEmailAsync(string to, string userName);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendEmailConfirmationAsync(string to, string confirmationToken);
    }
}
