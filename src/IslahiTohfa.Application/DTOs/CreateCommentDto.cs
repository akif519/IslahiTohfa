using System.ComponentModel.DataAnnotations;

namespace IslahiTohfa.Application.DTOs;

public class CreateCommentDto
{
    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Comment must be between 10 and 1000 characters")]
    public string Content { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
    
    [Required]
    public int BookId { get; set; }
}
