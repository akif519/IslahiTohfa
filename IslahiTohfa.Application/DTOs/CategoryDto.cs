using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameArabic { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string NameArabic { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string IconClass { get; set; }

        public int SortOrder { get; set; }
    }

    public class UpdateCategoryDto
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string NameArabic { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string IconClass { get; set; }

        public int? SortOrder { get; set; }

        public bool? IsActive { get; set; }
    }
}
