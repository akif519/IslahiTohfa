using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IBookService bookService, ILogger<CategoriesController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _bookService.GetCategoriesWithBookCountAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}/books")]
        public async Task<ActionResult<PaginatedResult<BookDto>>> GetCategoryBooks(
            int id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var criteria = new BookSearchCriteria
                {
                    CategoryId = id,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var result = await _bookService.SearchBooksAsync(criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting books for category {CategoryId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
