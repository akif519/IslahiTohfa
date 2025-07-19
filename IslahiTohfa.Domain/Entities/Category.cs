using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    public string NameArabic { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public string IconClass { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
}
