using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class BookAnalyticsDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public TimeSpan AverageReadingTime { get; set; }
        public List<AnalyticsDataDto> DailyViews { get; set; } = new();
        public List<AnalyticsDataDto> DailyDownloads { get; set; } = new();
    }
}
