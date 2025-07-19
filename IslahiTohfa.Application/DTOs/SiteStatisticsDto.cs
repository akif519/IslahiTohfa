using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class SiteStatisticsDto
    {
        public int TotalBooks { get; set; }
        public int TotalUsers { get; set; }
        public int TotalDownloads { get; set; }
        public int TotalViews { get; set; }
        public int TotalComments { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
    }
}
