using IslahiTohfa.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileStorageService fileStorageService, ILogger<FileController> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        [HttpPost("upload-book")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FileUploadDto>> UploadBookFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                // Validate file type
                var allowedTypes = new[] { "application/pdf" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest(new { message = "Only PDF files are allowed" });
                }

                // Validate file size (max 50MB)
                if (file.Length > 50 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size too large. Maximum 50MB allowed." });
                }

                var result = await _fileStorageService.UploadFileAsync(file, "books");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading book file");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("upload-thumbnail")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FileUploadDto>> UploadThumbnail(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest(new { message = "Only JPEG, PNG, and WebP images are allowed" });
                }

                // Validate file size (max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size too large. Maximum 5MB allowed." });
                }

                var result = await _fileStorageService.UploadFileAsync(file, "thumbnails");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading thumbnail");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }

    // Helper classes for request models
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}
