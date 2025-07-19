using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<BookDto>>> GetBooks([FromQuery] BookSearchCriteria criteria)
        {
            try
            {
                var result = await _bookService.SearchBooksAsync(criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting books");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<BookDto>>> GetFeaturedBooks([FromQuery] int count = 6)
        {
            try
            {
                var books = await _bookService.GetFeaturedBooksAsync(count);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured books");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("top-rated")]
        public async Task<ActionResult<List<BookDto>>> GetTopRatedBooks([FromQuery] int count = 6)
        {
            try
            {
                var books = await _bookService.GetTopRatedBooksAsync(count);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top rated books");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("most-viewed")]
        public async Task<ActionResult<List<BookDto>>> GetMostViewedBooks([FromQuery] int count = 6)
        {
            try
            {
                var books = await _bookService.GetMostViewedBooksAsync(count);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most viewed books");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("recent")]
        public async Task<ActionResult<List<BookDto>>> GetRecentBooks([FromQuery] int count = 6)
        {
            try
            {
                var books = await _bookService.GetRecentBooksAsync(count);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent books");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            try
            {
                var book = await _bookService.CreateBookAsync(createBookDto);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            try
            {
                var book = await _bookService.UpdateBookAsync(id, updateBookDto);
                return Ok(book);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id);

                if (success)
                {
                    return NoContent();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<ActionResult<bool>> ToggleLike(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var isLiked = await _bookService.ToggleLikeAsync(id, userId);
                return Ok(new { isLiked });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling like for book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("{id}/rate")]
        [Authorize]
        public async Task<ActionResult<RatingDto>> RateBook(int id, [FromBody] CreateRatingDto ratingDto)
        {
            try
            {
                ratingDto.BookId = id;
                ratingDto.UserId = GetCurrentUserId();

                var rating = await _bookService.AddOrUpdateRatingAsync(ratingDto);
                return Ok(rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rating book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<CommentDto>>> GetBookComments(int id)
        {
            try
            {
                var comments = await _bookService.GetBookCommentsAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments for book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("{id}/comments")]
        [Authorize]
        public async Task<ActionResult<CommentDto>> AddComment(int id, [FromBody] CreateCommentDto commentDto)
        {
            try
            {
                commentDto.BookId = id;
                commentDto.UserId = GetCurrentUserId();

                var comment = await _bookService.AddCommentAsync(commentDto);
                return CreatedAtAction(nameof(GetBookComments), new { id }, comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to book {BookId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("track-activity")]
        [Authorize]
        public async Task<ActionResult> TrackActivity([FromBody] UserActivityDto activityDto)
        {
            try
            {
                activityDto.UserId = GetCurrentUserId();
                await _bookService.TrackUserActivityAsync(activityDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking user activity");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}
