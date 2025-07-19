using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Services
{
    public interface IBookService
    {
        Task<List<BookDto>> GetFeaturedBooksAsync(int count = 6);
        Task<List<BookDto>> GetTopRatedBooksAsync(int count = 6);
        Task<List<BookDto>> GetMostViewedBooksAsync(int count = 6);
        Task<List<BookDto>> GetRecentBooksAsync(int count = 6);
        Task<List<CategoryDto>> GetCategoriesWithBookCountAsync();
        Task<SiteStatisticsDto> GetSiteStatisticsAsync();
        Task<BookDto> GetBookByIdAsync(int bookId);
        Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
        Task<BookDto> UpdateBookAsync(int bookId, UpdateBookDto updateBookDto);
        Task<bool> DeleteBookAsync(int bookId);
        Task<PaginatedResult<BookDto>> SearchBooksAsync(BookSearchCriteria criteria);
        Task TrackUserActivityAsync(UserActivityDto activityDto);
        Task<List<CommentDto>> GetBookCommentsAsync(int bookId);
        Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto);
        Task<RatingDto> AddOrUpdateRatingAsync(CreateRatingDto ratingDto);
        Task<bool> ToggleLikeAsync(int bookId, int userId);
    }
}
