using Microsoft.AspNetCore.Http;

namespace IslahiTohfa.Application.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);
    Task<bool> DeleteFileAsync(string filePath);
    Task<byte[]> ReadFileAsync(string filePath);
    Task<string> GetFileContentTypeAsync(string filePath);
    bool FileExists(string filePath);
    long GetFileSize(string filePath);
    Task<string> SaveBookPdfAsync(IFormFile pdfFile);
    Task<string> SaveBookCoverAsync(IFormFile imageFile);
}
