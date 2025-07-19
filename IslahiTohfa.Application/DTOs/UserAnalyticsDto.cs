using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class UserAnalyticsDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BooksRead { get; set; }
        public int CommentsPosted { get; set; }
        public int RatingsGiven { get; set; }
        public int LikesGiven { get; set; }
        public TimeSpan TotalReadingTime { get; set; }
        public DateTime LastActivity { get; set; }
        public List<BookSummaryDto> FavoriteBooks { get; set; } = new();
        public List<AnalyticsDataDto> ReadingHistory { get; set; } = new();
    }
}
