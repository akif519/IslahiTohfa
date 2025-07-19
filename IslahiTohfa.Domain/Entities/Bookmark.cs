using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Entities
{
    public class Bookmark
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public int PageNumber { get; set; }

        public string Note { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
