using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string PdfFilePath { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameArabic { get; set; }
        public BookStatus Status { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public int TotalLikes { get; set; }
        public int CommentsCount { get; set; }
        public string Language { get; set; }
        public long FileSizeBytes { get; set; }
        public int PageCount { get; set; }
        public string ISBN { get; set; }
        public bool IsFeatured { get; set; }

        // User-specific properties
        public bool IsLikedByCurrentUser { get; set; }
        public int? CurrentUserRating { get; set; }

        // Additional properties
        public List<string> Tags { get; set; } = new();
        public List<BookSummaryDto> RelatedBooks { get; set; } = new();

        // Formatted properties
        public string FormattedFileSize => FormatFileSize(FileSizeBytes);
        public string FormattedRating => AverageRating.ToString("F1");

        private static string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            double number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }
            return $"{number:N1} {suffixes[counter]}";
        }
    }
    public class BookSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ThumbnailUrl { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int ViewCount { get; set; }
    }

    public class CreateBookDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        [Required]
        public string PdfFilePath { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string Language { get; set; } = "ar";

        public long FileSizeBytes { get; set; }

        public int PageCount { get; set; }

        public string ISBN { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class UpdateBookDto
    {
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public string ThumbnailUrl { get; set; }

        public int? CategoryId { get; set; }

        public BookStatus? Status { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string Language { get; set; }

        public int? PageCount { get; set; }

        public string ISBN { get; set; }

        public bool? IsFeatured { get; set; }
    }
}
