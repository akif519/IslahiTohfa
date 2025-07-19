using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Rating> _ratingRepository;
        private readonly IGenericRepository<Like> _likeRepository;
        private readonly IGenericRepository<UserActivity> _activityRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BookService> _logger;

        public BookService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<Comment> commentRepository,
            IGenericRepository<Rating> ratingRepository,
            IGenericRepository<Like> likeRepository,
            IGenericRepository<UserActivity> activityRepository,
            IMapper mapper,
            IMemoryCache cache,
            ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _ratingRepository = ratingRepository;
            _likeRepository = likeRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<BookDto>> GetFeaturedBooksAsync(int count = 6)
        {
            const string cacheKey = "featured_books";

            if (_cache.TryGetValue(cacheKey, out List<BookDto> cachedBooks))
            {
                return cachedBooks.Take(count).ToList();
            }

            try
            {
                var books = await _bookRepository.GetFeaturedBooksAsync(count);
                var bookDtos = _mapper.Map<List<BookDto>>(books);

                _cache.Set(cacheKey, bookDtos, TimeSpan.FromMinutes(30));
                return bookDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured books");
                throw;
            }
        }

        public async Task<List<BookDto>> GetTopRatedBooksAsync(int count = 6)
        {
            const string cacheKey = "top_rated_books";

            if (_cache.TryGetValue(cacheKey, out List<BookDto> cachedBooks))
            {
                return cachedBooks.Take(count).ToList();
            }

            try
            {
                var books = await _bookRepository.GetTopRatedBooksAsync(count);
                var bookDtos = _mapper.Map<List<BookDto>>(books);

                _cache.Set(cacheKey, bookDtos, TimeSpan.FromMinutes(30));
                return bookDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top rated books");
                throw;
            }
        }

        public async Task<List<BookDto>> GetMostViewedBooksAsync(int count = 6)
        {
            const string cacheKey = "most_viewed_books";

            if (_cache.TryGetValue(cacheKey, out List<BookDto> cachedBooks))
            {
                return cachedBooks.Take(count).ToList();
            }

            try
            {
                var books = await _bookRepository.GetMostViewedBooksAsync(count);
                var bookDtos = _mapper.Map<List<BookDto>>(books);

                _cache.Set(cacheKey, bookDtos, TimeSpan.FromMinutes(30));
                return bookDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most viewed books");
                throw;
            }
        }

        public async Task<List<BookDto>> GetRecentBooksAsync(int count = 6)
        {
            try
            {
                var books = await _bookRepository.GetRecentBooksAsync(count);
                return _mapper.Map<List<BookDto>>(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent books");
                throw;
            }
        }

        public async Task<List<CategoryDto>> GetCategoriesWithBookCountAsync()
        {
            const string cacheKey = "categories_with_count";

            if (_cache.TryGetValue(cacheKey, out List<CategoryDto> cachedCategories))
            {
                return cachedCategories;
            }

            try
            {
                var categories = await _categoryRepository.GetAllAsync(
                    filter: c => c.IsActive,
                    orderBy: q => q.OrderBy(c => c.SortOrder),
                    includeProperties: "Books"
                );

                var categoryDtos = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    NameArabic = c.NameArabic,
                    Description = c.Description,
                    IconClass = c.IconClass,
                    BookCount = c.Books.Count(b => b.IsActive && b.Status == BookStatus.Published)
                }).ToList();

                _cache.Set(cacheKey, categoryDtos, TimeSpan.FromHours(1));
                return categoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories with book count");
                throw;
            }
        }

        public async Task<SiteStatisticsDto> GetSiteStatisticsAsync()
        {
            const string cacheKey = "site_statistics";

            if (_cache.TryGetValue(cacheKey, out SiteStatisticsDto cachedStats))
            {
                return cachedStats;
            }

            try
            {
                var stats = new SiteStatisticsDto
                {
                    TotalBooks = await _bookRepository.CountAsync(b => b.IsActive && b.Status == BookStatus.Published),
                    TotalUsers = await _userRepository.CountAsync(u => u.IsActive),
                    TotalDownloads = await _activityRepository.CountAsync(a => a.ActivityType == ActivityType.BookDownloaded),
                    TotalViews = await _activityRepository.CountAsync(a => a.ActivityType == ActivityType.BookViewed)
                };

                _cache.Set(cacheKey, stats, TimeSpan.FromMinutes(15));
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site statistics");
                throw;
            }
        }

        public async Task<BookDto> GetBookByIdAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(
                    bookId,
                    includeProperties: "Category,Comments.User,Ratings"
                );

                if (book == null)
                    throw new KeyNotFoundException($"Book with ID {bookId} not found");

                return _mapper.Map<BookDto>(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book by ID: {BookId}", bookId);
                throw;
            }
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            try
            {
                var book = _mapper.Map<Book>(createBookDto);
                book.CreatedDate = DateTime.UtcNow;
                book.UpdatedDate = DateTime.UtcNow;
                book.Status = BookStatus.Draft;

                await _bookRepository.AddAsync(book);
                await _bookRepository.SaveChangesAsync();

                // Clear cache
                _cache.Remove("featured_books");
                _cache.Remove("top_rated_books");
                _cache.Remove("most_viewed_books");

                return _mapper.Map<BookDto>(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                throw;
            }
        }

        public async Task<BookDto> UpdateBookAsync(int bookId, UpdateBookDto updateBookDto)
        {
            try
            {
                var existingBook = await _bookRepository.GetByIdAsync(bookId);
                if (existingBook == null)
                    throw new KeyNotFoundException($"Book with ID {bookId} not found");

                _mapper.Map(updateBookDto, existingBook);
                existingBook.UpdatedDate = DateTime.UtcNow;

                await _bookRepository.UpdateAsync(existingBook);
                await _bookRepository.SaveChangesAsync();

                // Clear cache
                _cache.Remove("featured_books");
                _cache.Remove("top_rated_books");
                _cache.Remove("most_viewed_books");

                return _mapper.Map<BookDto>(existingBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book: {BookId}", bookId);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                    return false;

                // Soft delete
                book.IsActive = false;
                book.UpdatedDate = DateTime.UtcNow;

                await _bookRepository.UpdateAsync(book);
                await _bookRepository.SaveChangesAsync();

                // Clear cache
                _cache.Remove("featured_books");
                _cache.Remove("top_rated_books");
                _cache.Remove("most_viewed_books");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book: {BookId}", bookId);
                throw;
            }
        }

        public async Task<PaginatedResult<BookDto>> SearchBooksAsync(BookSearchCriteria criteria)
        {
            try
            {
                var result = await _bookRepository.SearchBooksAsync(criteria);

                return new PaginatedResult<BookDto>
                {
                    Items = _mapper.Map<List<BookDto>>(result.Items),
                    TotalCount = result.TotalCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalPages = result.TotalPages
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books");
                throw;
            }
        }

        public async Task TrackUserActivityAsync(UserActivityDto activityDto)
        {
            try
            {
                var activity = _mapper.Map<UserActivity>(activityDto);
                activity.ActivityDate = DateTime.UtcNow;

                await _activityRepository.AddAsync(activity);
                await _activityRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking user activity");
                // Don't throw here as activity tracking shouldn't break main functionality
            }
        }

        public async Task<List<CommentDto>> GetBookCommentsAsync(int bookId)
        {
            try
            {
                var comments = await _commentRepository.GetAllAsync(
                    filter: c => c.BookId == bookId && c.IsActive,
                    orderBy: q => q.OrderBy(c => c.CreatedDate),
                    includeProperties: "User,Replies.User"
                );

                return _mapper.Map<List<CommentDto>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book comments: {BookId}", bookId);
                throw;
            }
        }

        public async Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto)
        {
            try
            {
                var comment = _mapper.Map<Comment>(commentDto);
                comment.CreatedDate = DateTime.UtcNow;

                await _commentRepository.AddAsync(comment);
                await _commentRepository.SaveChangesAsync();

                // Track activity
                await TrackUserActivityAsync(new UserActivityDto
                {
                    UserId = comment.UserId,
                    BookId = comment.BookId,
                    ActivityType = ActivityType.BookCommented
                });

                var createdComment = await _commentRepository.GetByIdAsync(
                    comment.Id,
                    includeProperties: "User"
                );

                return _mapper.Map<CommentDto>(createdComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                throw;
            }
        }

        public async Task<RatingDto> AddOrUpdateRatingAsync(CreateRatingDto ratingDto)
        {
            try
            {
                var existingRating = await _ratingRepository.GetFirstOrDefaultAsync(
                    r => r.BookId == ratingDto.BookId && r.UserId == ratingDto.UserId
                );

                Rating rating;
                if (existingRating != null)
                {
                    existingRating.Value = ratingDto.Value;
                    existingRating.UpdatedDate = DateTime.UtcNow;
                    await _ratingRepository.UpdateAsync(existingRating);
                    rating = existingRating;
                }
                else
                {
                    rating = _mapper.Map<Rating>(ratingDto);
                    rating.CreatedDate = DateTime.UtcNow;
                    await _ratingRepository.AddAsync(rating);
                }

                await _ratingRepository.SaveChangesAsync();

                // Update book's average rating
                await UpdateBookAverageRating(ratingDto.BookId);

                // Track activity
                await TrackUserActivityAsync(new UserActivityDto
                {
                    UserId = rating.UserId,
                    BookId = rating.BookId,
                    ActivityType = ActivityType.BookRated,
                    AdditionalData = rating.Value.ToString()
                });

                return _mapper.Map<RatingDto>(rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding/updating rating");
                throw;
            }
        }

        public async Task<bool> ToggleLikeAsync(int bookId, int userId)
        {
            try
            {
                var existingLike = await _likeRepository.GetFirstOrDefaultAsync(
                    l => l.BookId == bookId && l.UserId == userId
                );

                bool isLiked;
                if (existingLike != null)
                {
                    await _likeRepository.DeleteAsync(existingLike);
                    isLiked = false;

                    await TrackUserActivityAsync(new UserActivityDto
                    {
                        UserId = userId,
                        BookId = bookId,
                        ActivityType = ActivityType.BookUnliked
                    });
                }
                else
                {
                    var like = new Like
                    {
                        BookId = bookId,
                        UserId = userId,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _likeRepository.AddAsync(like);
                    isLiked = true;

                    await TrackUserActivityAsync(new UserActivityDto
                    {
                        UserId = userId,
                        BookId = bookId,
                        ActivityType = ActivityType.BookLiked
                    });
                }

                await _likeRepository.SaveChangesAsync();

                // Update book's total likes count
                await UpdateBookLikesCount(bookId);

                return isLiked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling like for book: {BookId}, User: {UserId}", bookId, userId);
                throw;
            }
        }

        private async Task UpdateBookAverageRating(int bookId)
        {
            var ratings = await _ratingRepository.GetAllAsync(r => r.BookId == bookId);

            if (ratings.Any())
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                book.AverageRating = ratings.Average(r => r.Value);
                book.TotalRatings = ratings.Count();

                await _bookRepository.UpdateAsync(book);
                await _bookRepository.SaveChangesAsync();
            }
        }

        private async Task UpdateBookLikesCount(int bookId)
        {
            var likesCount = await _likeRepository.CountAsync(l => l.BookId == bookId);

            var book = await _bookRepository.GetByIdAsync(bookId);
            book.TotalLikes = likesCount;

            await _bookRepository.UpdateAsync(book);
            await _bookRepository.SaveChangesAsync();
        }
    }
}
