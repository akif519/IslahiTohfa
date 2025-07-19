using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IslahiTohfa.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool EmailConfirmed { get; set; }

        public string ProfileImageUrl { get; set; }

        public string PreferredLanguage { get; set; } = "ar";

        // Navigation Properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<UserActivity> Activities { get; set; } = new List<UserActivity>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}
