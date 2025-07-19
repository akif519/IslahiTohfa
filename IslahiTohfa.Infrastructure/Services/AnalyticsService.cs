using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Services;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookService _bookService;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<UserActivity> _activityRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly ILogger<AnalyticsService> _logger;
        public AnalyticsService(
            IBookService bookService,
            IUserRepository userRepository,
            IGenericRepository<UserActivity> activityRepository,
            IGenericRepository<Comment> commentRepository,
            ILogger<AnalyticsService> logger)
        {
            _bookService = bookService;
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            try
            {
                var siteStats = await _bookService.GetSiteStatisticsAsync();
                var popularBooks = await _bookService.GetMostViewedBooksAsync(8);
                var activeUsers = await _userRepository.GetActiveUsersAsync(5);
                var recentComments = await GetRecentCommentsAsync(6);
                var dailyActivities = await GetDailyActivitiesAsync(30);

                return new DashboardSummaryDto
                {
                    SiteStatistics = siteStats,
                    PopularBooks = popularBooks.Select(b => new BookSummaryDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        ThumbnailUrl = b.ThumbnailUrl,
                        CategoryName = b.CategoryName,
                        AverageRating = b.AverageRating,
                        ViewCount = b.ViewCount
                    }).ToList(),
                    ActiveUsers = activeUsers.ToList(),
                    RecentComments = recentComments,
                    DailyActivities = dailyActivities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard summary");
                throw;
            }
        }

        public async Task<List<AnalyticsDataDto>> GetAnalyticsDataAsync(string type, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;

                return type.ToLower() switch
                {
                    "views" => await GetViewsAnalyticsAsync(start, end),
                    "downloads" => await GetDownloadsAnalyticsAsync(start, end),
                    "registrations" => await GetRegistrationsAnalyticsAsync(start, end),
                    "comments" => await GetCommentsAnalyticsAsync(start, end),
                    _ => new List<AnalyticsDataDto>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analytics data for type: {Type}", type);
                throw;
            }
        }

        private async Task<List<CommentDto>> GetRecentCommentsAsync(int count)
        {
            var comments = await _commentRepository.GetAllAsync(
                filter: c => c.IsActive,
                orderBy: q => q.OrderByDescending(c => c.CreatedDate),
                includeProperties: "User,Book"
            );

            return comments.Take(count).Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                BookId = c.BookId,
                BookTitle = c.Book?.Title,
                UserId = c.UserId,
                UserFullName = $"{c.User?.FirstName} {c.User?.LastName}".Trim(),
                CreatedDate = c.CreatedDate
            }).ToList();
        }

        private async Task<List<AnalyticsDataDto>> GetDailyActivitiesAsync(int days)
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;
            var activities = await _activityRepository.GetAllAsync(
                filter: a => a.ActivityDate >= startDate && a.ActivityType == ActivityType.BookViewed
            );

            return activities
                .GroupBy(a => a.ActivityDate.Date)
                .Select(g => new AnalyticsDataDto
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("dd/MM")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        private async Task<List<AnalyticsDataDto>> GetViewsAnalyticsAsync(DateTime start, DateTime end)
        {
            var activities = await _activityRepository.GetAllAsync(
                filter: a => a.ActivityDate >= start &&
                           a.ActivityDate <= end &&
                           a.ActivityType == ActivityType.BookViewed
            );

            return activities
                .GroupBy(a => a.ActivityDate.Date)
                .Select(g => new AnalyticsDataDto
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("dd/MM")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        private async Task<List<AnalyticsDataDto>> GetDownloadsAnalyticsAsync(DateTime start, DateTime end)
        {
            var activities = await _activityRepository.GetAllAsync(
                filter: a => a.ActivityDate >= start &&
                           a.ActivityDate <= end &&
                           a.ActivityType == ActivityType.BookDownloaded
            );

            return activities
                .GroupBy(a => a.ActivityDate.Date)
                .Select(g => new AnalyticsDataDto
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("dd/MM")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        private async Task<List<AnalyticsDataDto>> GetRegistrationsAnalyticsAsync(DateTime start, DateTime end)
        {
            var users = await _userRepository.GetAllAsync(
                filter: u => u.CreatedDate >= start && u.CreatedDate <= end
            );

            return users
                .GroupBy(u => u.CreatedDate.Date)
                .Select(g => new AnalyticsDataDto
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("dd/MM")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        private async Task<List<AnalyticsDataDto>> GetCommentsAnalyticsAsync(DateTime start, DateTime end)
        {
            var comments = await _commentRepository.GetAllAsync(
                filter: c => c.CreatedDate >= start && c.CreatedDate <= end && c.IsActive
            );

            return comments
                .GroupBy(c => c.CreatedDate.Date)
                .Select(g => new AnalyticsDataDto
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("dd/MM")
                })
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<BookAnalyticsDto> GetBookAnalyticsAsync(int bookId)
        {
            // Implementation for detailed book analytics
            throw new NotImplementedException();
        }

        public async Task<UserAnalyticsDto> GetUserAnalyticsAsync(int userId)
        {
            // Implementation for detailed user analytics
            throw new NotImplementedException();
        }

        public async Task ProcessDailyAnalyticsAsync()
        {
            // Process and aggregate daily analytics data
            _logger.LogInformation("Processing daily analytics...");
            await Task.CompletedTask;
        }
    }
}
