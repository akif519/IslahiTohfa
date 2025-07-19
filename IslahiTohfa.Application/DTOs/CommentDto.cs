using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ParentCommentId { get; set; }
        public int RepliesCount { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
    }

    public class CreateCommentDto
    {
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? ParentCommentId { get; set; }
    }

    public class UpdateCommentDto
    {
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
    }
}
