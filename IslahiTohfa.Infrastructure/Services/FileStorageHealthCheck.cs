using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class FileStorageHealthCheck : IHealthCheck
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageHealthCheck(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Test write access
                var testFile = Path.Combine(uploadPath, "health_test.txt");
                File.WriteAllText(testFile, "health check");
                File.Delete(testFile);

                return Task.FromResult(HealthCheckResult.Healthy("File storage is accessible"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("File storage is not accessible", ex));
            }
        }
    }
}
