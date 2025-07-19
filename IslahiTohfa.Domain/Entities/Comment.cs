using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public int? ParentCommentId { get; set; }

        // Navigation Properties
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
