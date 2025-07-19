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
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(200)]
        public string ThumbnailUrl { get; set; }

        [Required]
        [StringLength(500)]
        public string PdfFilePath { get; set; }

        public int CategoryId { get; set; }

        public BookStatus Status { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int ViewCount { get; set; }

        public int DownloadCount { get; set; }

        public double AverageRating { get; set; }

        public int TotalRatings { get; set; }

        public int TotalLikes { get; set; }

        public string Language { get; set; } = "ar"; // Arabic by default

        public long FileSizeBytes { get; set; }

        public int PageCount { get; set; }

        [StringLength(50)]
        public string ISBN { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        // Navigation Properties
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}
