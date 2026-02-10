using IslahiTohfa.Domain.Common;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5 stars
    
    // Foreign Keys
    public int BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Moderation
    public CommentStatus Status { get; set; } = CommentStatus.Pending;
    public string? ModeratorNotes { get; set; }
    public string? ModeratedBy { get; set; }
    public DateTime? ModeratedAt { get; set; }
    
    // Navigation Properties
    public virtual Book Book { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}
