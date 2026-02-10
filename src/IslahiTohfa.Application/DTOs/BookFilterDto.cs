using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.DTOs;

public class BookFilterDto
{
    public string? SearchTerm { get; set; }
    public BookCategory? Category { get; set; }
    public string? Language { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public BookStatus? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public string SortBy { get; set; } = "CreatedAt"; // CreatedAt, Title, ViewCount, LikeCount
    public string SortOrder { get; set; } = "desc"; // asc, desc
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
