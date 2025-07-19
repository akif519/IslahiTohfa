using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Services;
using IslahiTohfa.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAnalyticsService analyticsService,
            IBookService bookService,
            IUserService userService,
            ILogger<AdminController> logger)
        {
            _analyticsService = analyticsService;
            _bookService = bookService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary()
        {
            try
            {
                var dashboard = await _analyticsService.GetDashboardSummaryAsync();
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard summary");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<SiteStatisticsDto>> GetSiteStatistics()
        {
            try
            {
                var stats = await _bookService.GetSiteStatisticsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site statistics");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<List<AnalyticsDataDto>>> GetAnalytics(
            [FromQuery] string type,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var analytics = await _analyticsService.GetAnalyticsDataAsync(type, startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analytics data");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<PaginatedResult<UserDto>>> GetUsers(
            [FromQuery] string searchTerm = "",
            [FromQuery] UserRole? role = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(searchTerm, role, pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("users/{id}/toggle-status")]
        public async Task<ActionResult> ToggleUserStatus(int id)
        {
            try
            {
                await _userService.ToggleUserStatusAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling user status {UserId}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
