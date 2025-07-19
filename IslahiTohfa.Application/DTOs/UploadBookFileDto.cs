using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class UploadBookFileDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string Language { get; set; } = "ar";

        public bool IsFeatured { get; set; }

        // File data will be handled separately in the controller
    }
}
