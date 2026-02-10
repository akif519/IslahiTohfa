using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<PaginatedList<Book>> GetFilteredBooksAsync(BookFilterDto filter);
    Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 6);
    Task<IEnumerable<Book>> GetRecentBooksAsync(int count = 10);
    Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(BookCategory category, int count = 20);
    Task<Book?> GetBookWithDetailsAsync(int id);
    Task IncrementViewCountAsync(int bookId);
    Task IncrementDownloadCountAsync(int bookId);
    Task<bool> IsBookLikedByUserAsync(int bookId, string userId);
}
