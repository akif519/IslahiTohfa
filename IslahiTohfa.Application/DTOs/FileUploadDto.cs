using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.DTOs
{
    public class FileUploadDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
