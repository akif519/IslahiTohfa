using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Domain.Entities
{
    public class UserActivity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public ActivityType ActivityType { get; set; }

        public DateTime ActivityDate { get; set; }

        public string AdditionalData { get; set; } // JSON for storing activity-specific data

        public int? PageNumber { get; set; } // For reading progress

        public TimeSpan? ReadingDuration { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
