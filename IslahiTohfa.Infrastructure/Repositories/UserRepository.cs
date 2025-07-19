using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
        }

        public async Task<User> GetByEmailOrUsernameAsync(string emailOrUsername)
        {
            var identifier = emailOrUsername.ToLower();
            return await _dbSet.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == identifier ||
                u.UserName.ToLower() == identifier);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync(int count = 10)
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.LastLoginDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _dbSet
                .Where(u => u.IsActive && u.Role == role)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync();
        }

        public async Task<UserAnalyticsDto> GetUserAnalyticsAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return null;

            var activities = await _context.UserActivities
                .Where(a => a.UserId == userId)
                .ToListAsync();

            var comments = await _context.Comments
                .Where(c => c.UserId == userId && c.IsActive)
                .CountAsync();

            var ratings = await _context.Ratings
                .Where(r => r.UserId == userId)
                .CountAsync();

            var likes = await _context.Likes
                .Where(l => l.UserId == userId)
                .CountAsync();

            var booksRead = activities
                .Where(a => a.ActivityType == ActivityType.ReadingCompleted)
                .Select(a => a.BookId)
                .Distinct()
                .Count();

            var totalReadingTime = activities
                .Where(a => a.ReadingDuration.HasValue)
                .Sum(a => a.ReadingDuration.Value.TotalMinutes);

            var favoriteBooks = await GetUserFavoriteBooksAsync(userId, 5);

            return new UserAnalyticsDto
            {
                UserId = userId,
                UserName = user.UserName,
                BooksRead = booksRead,
                CommentsPosted = comments,
                RatingsGiven = ratings,
                LikesGiven = likes,
                TotalReadingTime = TimeSpan.FromMinutes(totalReadingTime),
                LastActivity = activities.Max(a => a.ActivityDate),
                FavoriteBooks = favoriteBooks.Select(b => new BookSummaryDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ThumbnailUrl = b.ThumbnailUrl,
                    CategoryName = b.Category?.Name,
                    AverageRating = b.AverageRating,
                    ViewCount = b.ViewCount
                }).ToList()
            };
        }

        private async Task<IEnumerable<Book>> GetUserFavoriteBooksAsync(int userId, int count)
        {
            return await _context.Likes
                .Where(l => l.UserId == userId)
                .Include(l => l.Book)
                .ThenInclude(b => b.Category)
                .Where(l => l.Book.IsActive && l.Book.Status == BookStatus.Published)
                .OrderByDescending(l => l.CreatedDate)
                .Select(l => l.Book)
                .Take(count)
                .ToListAsync();
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLoginDate = DateTime.UtcNow;
                await UpdateAsync(user);
                await SaveChangesAsync();
            }
        }

        public async Task<PaginatedResult<User>> SearchUsersAsync(string searchTerm, UserRole? role, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(u => u.IsActive).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.FirstName.ToLower().Contains(search) ||
                    u.LastName.ToLower().Contains(search));
            }

            if (role.HasValue)
            {
                query = query.Where(u => u.Role == role.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
