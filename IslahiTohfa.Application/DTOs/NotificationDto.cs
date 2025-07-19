using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ActionUrl { get; set; }
    }
}
