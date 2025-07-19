using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class AnalyticsDataDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public double Value { get; set; }
        public string Label { get; set; }
    }
}
