using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IslahiTohfa.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetCommentsByBookIdAsync(int bookId)
    {
        return await _dbSet
            .Where(c => c.BookId == bookId && c.Status == CommentStatus.Approved)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId)
    {
        return await _dbSet
            .Where(c => c.UserId == userId)
            .Include(c => c.Book)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetPendingCommentsAsync()
    {
        return await _dbSet
            .Where(c => c.Status == CommentStatus.Pending)
            .Include(c => c.Book)
            .Include(c => c.User)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<PaginatedList<Comment>> GetCommentsByStatusAsync(CommentStatus status, int pageNumber = 1, int pageSize = 20)
    {
        var query = _dbSet
            .Where(c => c.Status == status)
            .Include(c => c.Book)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Comment>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<double> GetAverageRatingForBookAsync(int bookId)
    {
        var ratings = await _dbSet
            .Where(c => c.BookId == bookId && c.Status == CommentStatus.Approved)
            .Select(c => c.Rating)
            .ToListAsync();

        return ratings.Any() ? ratings.Average() : 0;
    }

    public async Task ApproveCommentAsync(int commentId, string moderatorId, string? notes = null)
    {
        var comment = await _dbSet.FindAsync(commentId);
        if (comment != null)
        {
            comment.Status = CommentStatus.Approved;
            comment.ModeratedBy = moderatorId;
            comment.ModeratedAt = DateTime.UtcNow;
            comment.ModeratorNotes = notes;
            
            // Update book comment count
            var book = await _context.Books.FindAsync(comment.BookId);
            if (book != null)
            {
                book.CommentCount = await _dbSet
                    .CountAsync(c => c.BookId == book.Id && c.Status == CommentStatus.Approved);
            }
        }
    }

    public async Task RejectCommentAsync(int commentId, string moderatorId, string notes)
    {
        var comment = await _dbSet.FindAsync(commentId);
        if (comment != null)
        {
            comment.Status = CommentStatus.Rejected;
            comment.ModeratedBy = moderatorId;
            comment.ModeratedAt = DateTime.UtcNow;
            comment.ModeratorNotes = notes;
        }
    }
}
