using IslahiTohfa.Application.DTOs;

namespace IslahiTohfa.Application.Interfaces;

public interface IBookService
{
    Task<PaginatedList<BookDto>> GetBooksAsync(BookFilterDto filter);
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<BookDto> CreateBookAsync(CreateBookDto createDto, string userId);
    Task<BookDto> UpdateBookAsync(UpdateBookDto updateDto, string userId);
    Task<bool> DeleteBookAsync(int id);
    Task<IEnumerable<BookDto>> GetFeaturedBooksAsync(int count = 6);
    Task<IEnumerable<BookDto>> GetRecentBooksAsync(int count = 10);
    Task<IEnumerable<BookDto>> GetPopularBooksAsync(int count = 10);
    Task IncrementViewCountAsync(int bookId);
    Task<byte[]> GetBookPdfAsync(int bookId);
}
