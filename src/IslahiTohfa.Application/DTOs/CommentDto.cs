using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? UserProfilePicture { get; set; }
    public CommentStatus Status { get; set; }
    public string? ModeratorNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string StatusName => Status.ToString();
    public string CreatedTimeAgo => GetTimeAgo(CreatedAt);
    
    private string GetTimeAgo(DateTime date)
    {
        var timeSpan = DateTime.UtcNow - date;
        if (timeSpan.TotalMinutes < 1) return "just now";
        if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}m ago";
        if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}h ago";
        if (timeSpan.TotalDays < 30) return $"{(int)timeSpan.TotalDays}d ago";
        if (timeSpan.TotalDays < 365) return $"{(int)(timeSpan.TotalDays / 30)}mo ago";
        return $"{(int)(timeSpan.TotalDays / 365)}y ago";
    }
}
