using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IslahiTohfa.Infrastructure.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<Book>> GetFilteredBooksAsync(BookFilterDto filter)
    {
        var query = _dbSet.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(b => 
                b.Title.ToLower().Contains(searchTerm) ||
                b.Author.ToLower().Contains(searchTerm) ||
                (b.Description != null && b.Description.ToLower().Contains(searchTerm)));
        }

        if (filter.Category.HasValue)
            query = query.Where(b => b.Category == filter.Category.Value);

        if (!string.IsNullOrWhiteSpace(filter.Language))
            query = query.Where(b => b.Language == filter.Language);

        if (filter.MinYear.HasValue)
            query = query.Where(b => b.PublicationYear >= filter.MinYear.Value);

        if (filter.MaxYear.HasValue)
            query = query.Where(b => b.PublicationYear <= filter.MaxYear.Value);

        if (filter.Status.HasValue)
            query = query.Where(b => b.Status == filter.Status.Value);

        if (filter.IsFeatured.HasValue)
            query = query.Where(b => b.IsFeatured == filter.IsFeatured.Value);

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = filter.SortBy.ToLower() switch
        {
            "title" => filter.SortOrder.ToLower() == "asc" 
                ? query.OrderBy(b => b.Title) 
                : query.OrderByDescending(b => b.Title),
            "viewcount" => filter.SortOrder.ToLower() == "asc" 
                ? query.OrderBy(b => b.ViewCount) 
                : query.OrderByDescending(b => b.ViewCount),
            "likecount" => filter.SortOrder.ToLower() == "asc" 
                ? query.OrderBy(b => b.LikeCount) 
                : query.OrderByDescending(b => b.LikeCount),
            _ => filter.SortOrder.ToLower() == "asc" 
                ? query.OrderBy(b => b.CreatedAt) 
                : query.OrderByDescending(b => b.CreatedAt),
        };

        // Apply pagination
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(b => b.Comments)
            .Include(b => b.Likes)
            .ToListAsync();

        return new PaginatedList<Book>(items, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 6)
    {
        return await _dbSet
            .Where(b => b.IsFeatured && b.Status == BookStatus.Published)
            .OrderByDescending(b => b.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetRecentBooksAsync(int count = 10)
    {
        return await _dbSet
            .Where(b => b.Status == BookStatus.Published)
            .OrderByDescending(b => b.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10)
    {
        return await _dbSet
            .Where(b => b.Status == BookStatus.Published)
            .OrderByDescending(b => b.ViewCount)
            .ThenByDescending(b => b.LikeCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(BookCategory category, int count = 20)
    {
        return await _dbSet
            .Where(b => b.Category == category && b.Status == BookStatus.Published)
            .OrderByDescending(b => b.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Book?> GetBookWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Comments.Where(c => c.Status == CommentStatus.Approved))
                .ThenInclude(c => c.User)
            .Include(b => b.Likes)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task IncrementViewCountAsync(int bookId)
    {
        var book = await _dbSet.FindAsync(bookId);
        if (book != null)
        {
            book.ViewCount++;
            _context.Entry(book).Property(b => b.ViewCount).IsModified = true;
        }
    }

    public async Task IncrementDownloadCountAsync(int bookId)
    {
        var book = await _dbSet.FindAsync(bookId);
        if (book != null)
        {
            book.DownloadCount++;
            _context.Entry(book).Property(b => b.DownloadCount).IsModified = true;
        }
    }

    public async Task<bool> IsBookLikedByUserAsync(int bookId, string userId)
    {
        return await _context.Likes
            .AnyAsync(l => l.BookId == bookId && l.UserId == userId);
    }
}
