using IslahiTohfa.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileStorageService> _logger;
        private readonly FileStorageSettings _settings;

        public FileStorageService(
            IWebHostEnvironment environment,
            ILogger<FileStorageService> logger,
            IOptions<FileStorageSettings> settings)
        {
            _environment = environment;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<FileUploadDto> UploadFileAsync(IFormFile file, string folder)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is required");

                // Validate file size
                if (file.Length > _settings.MaxFileSize)
                    throw new ArgumentException($"File size exceeds maximum allowed size of {_settings.MaxFileSize / (1024 * 1024)}MB");

                // Validate file extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_settings.AllowedExtensions.Contains(extension))
                    throw new ArgumentException($"File type {extension} is not allowed");

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", folder);

                // Ensure directory exists
                Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                var relativePath = Path.Combine("uploads", folder, fileName).Replace("\\", "/");

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("File uploaded successfully: {FileName}", fileName);

                return new FileUploadDto
                {
                    FileName = fileName,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    FilePath = relativePath,
                    FileUrl = $"/{relativePath}",
                    UploadedDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {FileName}", file?.FileName);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
                return false;
            }
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    return await File.ReadAllBytesAsync(fullPath);
                }

                throw new FileNotFoundException($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading file: {FilePath}", filePath);
                throw;
            }
        }

        public async Task<string> GetFileUrlAsync(string filePath)
        {
            return await Task.FromResult($"/{filePath.TrimStart('/')}");
        }
    }
}
