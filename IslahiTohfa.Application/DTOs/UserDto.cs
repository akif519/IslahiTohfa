using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ProfileImageUrl { get; set; }
        public string PreferredLanguage { get; set; }

        // Statistics
        public int TotalBooks { get; set; }
        public int TotalComments { get; set; }
        public int TotalRatings { get; set; }
    }
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string PreferredLanguage { get; set; } = "ar";
    }

    public class LoginDto
    {
        [Required]
        public string EmailOrUsername { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class UpdateUserDto
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string PreferredLanguage { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
