using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class DashboardSummaryDto
    {
        public SiteStatisticsDto SiteStatistics { get; set; }
        public List<BookSummaryDto> PopularBooks { get; set; } = new();
        public List<UserDto> ActiveUsers { get; set; } = new();
        public List<CommentDto> RecentComments { get; set; } = new();
        public List<AnalyticsDataDto> DailyActivities { get; set; } = new();
        public List<AnalyticsDataDto> MonthlyTrends { get; set; } = new();
    }
}
