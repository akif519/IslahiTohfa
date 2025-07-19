using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<UserDto>> RegisterAsync(RegisterUserDto registerDto);
        Task<ApiResponse<string>> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync();
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UserDto> GetCurrentUserAsync();
        Task<bool> IsEmailAvailableAsync(string email);
        Task<bool> IsUsernameAvailableAsync(string username);
    }
}
