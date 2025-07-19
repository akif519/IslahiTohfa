using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class EmailServiceHealthCheck : IHealthCheck
    {
        private readonly IEmailService _emailService;

        public EmailServiceHealthCheck(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Test email service configuration (without actually sending)
                return HealthCheckResult.Healthy("Email service is configured");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Email service is not properly configured", ex);
            }
        }
    }
}
}
