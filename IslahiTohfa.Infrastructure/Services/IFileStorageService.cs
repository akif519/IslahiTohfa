using IslahiTohfa.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public interface IFileStorageService
    {
        Task<FileUploadDto> UploadFileAsync(IFormFile file, string folder);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> GetFileAsync(string filePath);
        Task<string> GetFileUrlAsync(string filePath);
    }
}
