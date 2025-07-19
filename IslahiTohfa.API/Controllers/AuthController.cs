using IslahiTohfa.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthenticationService authService,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (result.Success)
                {
                    return Ok(result);
                }

                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterUserDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<string>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(request.RefreshToken);

                if (result.Success)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var result = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in forgot password");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<string>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(resetPasswordDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in reset password");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            try
            {
                var user = await _authService.GetCurrentUserAsync();

                if (user != null)
                {
                    return Ok(user);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmailAvailability([FromQuery] string email)
        {
            try
            {
                var isAvailable = await _authService.IsEmailAvailableAsync(email);
                return Ok(new ApiResponse<bool> { Success = true, Data = isAvailable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email availability");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("check-username")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckUsernameAvailability([FromQuery] string username)
        {
            try
            {
                var isAvailable = await _authService.IsUsernameAvailableAsync(username);
                return Ok(new ApiResponse<bool> { Success = true, Data = isAvailable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username availability");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
