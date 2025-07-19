using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateRatingDto
    {
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    // Like DTOs
    public class LikeDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateLikeDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
