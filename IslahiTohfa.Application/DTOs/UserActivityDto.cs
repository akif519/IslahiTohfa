using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class UserActivityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public ActivityType ActivityType { get; set; }
        public string ActivityTypeName { get; set; }
        public DateTime ActivityDate { get; set; }
        public string AdditionalData { get; set; }
        public int? PageNumber { get; set; }
        public TimeSpan? ReadingDuration { get; set; }
    }
}
