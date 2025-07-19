using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
