using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(RegisterUserDto registerDto);
        Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto updateDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<PaginatedResult<UserDto>> SearchUsersAsync(string searchTerm, UserRole? role, int pageNumber, int pageSize);
        Task<bool> ToggleUserStatusAsync(int userId);
        Task<UserAnalyticsDto> GetUserAnalyticsAsync(int userId);
    }
}
