using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class BookSearchCriteria
    {
        public string Query { get; set; }
        public int? CategoryId { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public double? MinRating { get; set; }
        public BookStatus? Status { get; set; }
        public SortOrder SortBy { get; set; } = SortOrder.CreatedDate;
        public SortDirection SortDirection { get; set; } = SortDirection.Descending;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
