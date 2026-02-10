using IslahiTohfa.Domain.Common;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public int? PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public BookCategory Category { get; set; }
    public string? Language { get; set; }
    public int PageCount { get; set; }
    
    // File paths
    public string PdfFilePath { get; set; } = string.Empty;
    public string? CoverImagePath { get; set; }
    public long FileSize { get; set; } // in bytes
    
    // Statistics
    public int ViewCount { get; set; } = 0;
    public int DownloadCount { get; set; } = 0;
    public int LikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    
    // Status
    public BookStatus Status { get; set; } = BookStatus.Published;
    public bool IsFeatured { get; set; } = false;
    
    // Navigation Properties
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}
