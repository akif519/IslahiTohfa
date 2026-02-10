using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IslahiTohfa.Infrastructure.Repositories;

public class LikeRepository : Repository<Like>, ILikeRepository
{
    public LikeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Like?> GetLikeAsync(int bookId, string userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(l => l.BookId == bookId && l.UserId == userId);
    }

    public async Task<bool> ToggleLikeAsync(int bookId, string userId)
    {
        var existingLike = await GetLikeAsync(bookId, userId);
        
        if (existingLike != null)
        {
            // Unlike
            _dbSet.Remove(existingLike);
            
            // Update book like count
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.LikeCount = Math.Max(0, book.LikeCount - 1);
            }
            
            return false;
        }
        else
        {
            // Like
            var newLike = new Like
            {
                BookId = bookId,
                UserId = userId
            };
            await _dbSet.AddAsync(newLike);
            
            // Update book like count
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.LikeCount++;
            }
            
            return true;
        }
    }

    public async Task<int> GetLikeCountForBookAsync(int bookId)
    {
        return await _dbSet.CountAsync(l => l.BookId == bookId);
    }

    public async Task<IEnumerable<Like>> GetUserLikesAsync(string userId)
    {
        return await _dbSet
            .Where(l => l.UserId == userId)
            .Include(l => l.Book)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
}
