using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count)
        {
            return await _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published && b.IsFeatured)
                .Include(b => b.Category)
                .OrderByDescending(b => b.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetTopRatedBooksAsync(int count)
        {
            return await _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published && b.AverageRating > 0)
                .Include(b => b.Category)
                .OrderByDescending(b => b.AverageRating)
                .ThenByDescending(b => b.TotalRatings)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetMostViewedBooksAsync(int count)
        {
            return await _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published)
                .Include(b => b.Category)
                .OrderByDescending(b => b.ViewCount)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetRecentBooksAsync(int count)
        {
            return await _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published)
                .Include(b => b.Category)
                .OrderByDescending(b => b.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId, int count = 10)
        {
            return await _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published && b.CategoryId == categoryId)
                .Include(b => b.Category)
                .OrderByDescending(b => b.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<PaginatedResult<Book>> SearchBooksAsync(BookSearchCriteria criteria)
        {
            var query = _dbSet
                .Where(b => b.IsActive && b.Status == BookStatus.Published)
                .Include(b => b.Category)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(criteria.Query))
            {
                var searchTerm = criteria.Query.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Author.ToLower().Contains(searchTerm) ||
                    b.Description.ToLower().Contains(searchTerm));
            }

            if (criteria.CategoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == criteria.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(criteria.Author))
            {
                query = query.Where(b => b.Author.ToLower().Contains(criteria.Author.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Language))
            {
                query = query.Where(b => b.Language == criteria.Language);
            }

            if (criteria.MinRating.HasValue)
            {
                query = query.Where(b => b.AverageRating >= criteria.MinRating.Value);
            }

            // Apply sorting
            query = criteria.SortBy switch
            {
                SortOrder.Title => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.Title)
                    : query.OrderByDescending(b => b.Title),
                SortOrder.Author => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.Author)
                    : query.OrderByDescending(b => b.Author),
                SortOrder.Rating => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.AverageRating)
                    : query.OrderByDescending(b => b.AverageRating),
                SortOrder.ViewCount => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.ViewCount)
                    : query.OrderByDescending(b => b.ViewCount),
                SortOrder.DownloadCount => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.DownloadCount)
                    : query.OrderByDescending(b => b.DownloadCount),
                SortOrder.Popularity => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.ViewCount + b.DownloadCount + b.TotalLikes)
                    : query.OrderByDescending(b => b.ViewCount + b.DownloadCount + b.TotalLikes),
                _ => criteria.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(b => b.CreatedDate)
                    : query.OrderByDescending(b => b.CreatedDate)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new PaginatedResult<Book>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize
            };
        }

        public async Task<IEnumerable<Book>> GetRelatedBooksAsync(int bookId, int count = 5)
        {
            var book = await GetByIdAsync(bookId);
            if (book == null) return new List<Book>();

            return await _dbSet
                .Where(b => b.IsActive &&
                           b.Status == BookStatus.Published &&
                           b.Id != bookId &&
                           (b.CategoryId == book.CategoryId || b.Author == book.Author))
                .Include(b => b.Category)
                .OrderByDescending(b => b.AverageRating)
                .Take(count)
                .ToListAsync();
        }

        public async Task IncrementViewCountAsync(int bookId)
        {
            var book = await GetByIdAsync(bookId);
            if (book != null)
            {
                book.ViewCount++;
                await UpdateAsync(book);
                await SaveChangesAsync();
            }
        }

        public async Task IncrementDownloadCountAsync(int bookId)
        {
            var book = await GetByIdAsync(bookId);
            if (book != null)
            {
                book.DownloadCount++;
                await UpdateAsync(book);
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Book>> GetUserFavoriteBooksAsync(int userId, int count = 10)
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

        public async Task<IEnumerable<Book>> GetUserRecentlyViewedBooksAsync(int userId, int count = 10)
        {
            return await _context.UserActivities
                .Where(a => a.UserId == userId && a.ActivityType == ActivityType.BookViewed)
                .Include(a => a.Book)
                .ThenInclude(b => b.Category)
                .Where(a => a.Book.IsActive && a.Book.Status == BookStatus.Published)
                .OrderByDescending(a => a.ActivityDate)
                .Select(a => a.Book)
                .Distinct()
                .Take(count)
                .ToListAsync();
        }
    }
}
