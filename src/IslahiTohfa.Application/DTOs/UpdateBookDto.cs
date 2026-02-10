using IslahiTohfa.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IslahiTohfa.Application.DTOs;

public class UpdateBookDto
{
    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Author is required")]
    [StringLength(150, ErrorMessage = "Author name cannot exceed 150 characters")]
    public string Author { get; set; } = string.Empty;
    
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string? Description { get; set; }
    
    [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters")]
    public string? ISBN { get; set; }
    
    [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
    public int? PublicationYear { get; set; }
    
    [StringLength(150, ErrorMessage = "Publisher name cannot exceed 150 characters")]
    public string? Publisher { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public BookCategory Category { get; set; }
    
    [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters")]
    public string? Language { get; set; }
    
    [Range(1, 10000, ErrorMessage = "Page count must be between 1 and 10000")]
    public int PageCount { get; set; }
    
    // Optional: only if updating PDF
    public IFormFile? PdfFile { get; set; }
    
    // Optional: only if updating cover
    public IFormFile? CoverImage { get; set; }
    
    public BookStatus Status { get; set; }
    
    public bool IsFeatured { get; set; }
    
    // Existing file paths (for reference)
    public string? ExistingPdfPath { get; set; }
    public string? ExistingCoverPath { get; set; }
}
