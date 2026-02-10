using IslahiTohfa.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace IslahiTohfa.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadsFolder;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");

        // Ensure folders exist
        Directory.CreateDirectory(Path.Combine(_uploadsFolder, "pdfs"));
        Directory.CreateDirectory(Path.Combine(_uploadsFolder, "covers"));
        Directory.CreateDirectory(Path.Combine(_uploadsFolder, "profiles"));
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        var uploadsPath = Path.Combine(_uploadsFolder, folder);
        Directory.CreateDirectory(uploadsPath);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("uploads", folder, uniqueFileName).Replace("\\", "/");
    }

    public async Task<string> SaveBookPdfAsync(IFormFile pdfFile)
    {
        if (pdfFile == null || pdfFile.Length == 0)
            throw new ArgumentException("PDF file is required");

        var extension = Path.GetExtension(pdfFile.FileName).ToLowerInvariant();
        if (extension != ".pdf")
            throw new ArgumentException("Only PDF files are allowed");

        return await SaveFileAsync(pdfFile, "pdfs");
    }

    public async Task<string> SaveBookCoverAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            throw new ArgumentException("Image file is required");

        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        
        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException("Only image files (jpg, jpeg, png, gif, webp) are allowed");

        return await SaveFileAsync(imageFile, "covers");
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
            
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<byte[]> ReadFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
        
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found", filePath);

        return await File.ReadAllBytesAsync(fullPath);
    }

    public async Task<string> GetFileContentTypeAsync(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }

    public bool FileExists(string filePath)
    {
        var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
        return File.Exists(fullPath);
    }

    public long GetFileSize(string filePath)
    {
        var fullPath = Path.Combine(_environment.ContentRootPath, filePath);
        
        if (!File.Exists(fullPath))
            return 0;

        var fileInfo = new FileInfo(fullPath);
        return fileInfo.Length;
    }
}
