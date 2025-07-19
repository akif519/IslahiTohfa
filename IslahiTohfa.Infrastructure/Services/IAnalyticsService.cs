using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public interface IAnalyticsService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<AnalyticsDataDto>> GetAnalyticsDataAsync(string type, DateTime? startDate, DateTime? endDate);
        Task<BookAnalyticsDto> GetBookAnalyticsAsync(int bookId);
        Task<UserAnalyticsDto> GetUserAnalyticsAsync(int userId);
        Task ProcessDailyAnalyticsAsync();
    }
}
