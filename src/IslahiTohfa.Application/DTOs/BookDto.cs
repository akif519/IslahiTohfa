using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ISBN { get; set; }
    public int? PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public BookCategory Category { get; set; }
    public string? Language { get; set; }
    public int PageCount { get; set; }
    public string? CoverImagePath { get; set; }
    public long FileSize { get; set; }
    public int ViewCount { get; set; }
    public int DownloadCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public BookStatus Status { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Computed properties
    public string FileSizeFormatted => FormatFileSize(FileSize);
    public string CategoryName => Category.ToString();
    public string StatusName => Status.ToString();
    
    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
