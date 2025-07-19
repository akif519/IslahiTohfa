using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class FileUploadValidator : AbstractValidator<IFormFile>
    {
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions;

        public FileUploadValidator(long maxFileSize = 50 * 1024 * 1024, string[] allowedExtensions = null)
        {
            _maxFileSize = maxFileSize;
            _allowedExtensions = allowedExtensions ?? new[] { ".pdf", ".jpg", ".png", ".webp" };

            RuleFor(x => x)
                .NotNull().WithMessage("الملف مطلوب / File is required");

            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("الملف فارغ / File is empty")
                .LessThanOrEqualTo(_maxFileSize).WithMessage($"حجم الملف يجب أن يكون أقل من {_maxFileSize / (1024 * 1024)} ميجابايت / File size must be less than {_maxFileSize / (1024 * 1024)}MB");

            RuleFor(x => x.FileName)
                .Must(HaveValidExtension).WithMessage($"نوع الملف غير مدعوم. الأنواع المدعومة: {string.Join(", ", _allowedExtensions)} / File type not supported. Supported types: {string.Join(", ", _allowedExtensions)}");

            RuleFor(x => x.ContentType)
                .Must(HaveValidContentType).WithMessage("نوع محتوى الملف غير صحيح / Invalid file content type");
        }

        private bool HaveValidExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        private bool HaveValidContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;

            var validContentTypes = new[]
            {
                "application/pdf",
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/webp"
            };

            return validContentTypes.Contains(contentType.ToLowerInvariant());
        }
    }
}
