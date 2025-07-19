using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الكتاب مطلوب / Book title is required")
                .MaximumLength(200).WithMessage("عنوان الكتاب يجب أن يكون أقل من 200 حرف / Title must be less than 200 characters")
                .MinimumLength(3).WithMessage("عنوان الكتاب يجب أن يكون أكثر من 3 أحرف / Title must be more than 3 characters");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("اسم المؤلف مطلوب / Author name is required")
                .MaximumLength(100).WithMessage("اسم المؤلف يجب أن يكون أقل من 100 حرف / Author name must be less than 100 characters")
                .MinimumLength(2).WithMessage("اسم المؤلف يجب أن يكون أكثر من حرفين / Author name must be more than 2 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("الوصف يجب أن يكون أقل من 1000 حرف / Description must be less than 1000 characters");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("يجب اختيار فئة صحيحة / Valid category must be selected");

            RuleFor(x => x.PdfFilePath)
                .NotEmpty().WithMessage("ملف PDF مطلوب / PDF file is required")
                .Must(BeValidPdfPath).WithMessage("مسار ملف PDF غير صحيح / Invalid PDF file path");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("اللغة مطلوبة / Language is required")
                .Must(BeValidLanguage).WithMessage("اللغة غير صحيحة / Invalid language code");

            RuleFor(x => x.FileSizeBytes)
                .GreaterThan(0).WithMessage("حجم الملف يجب أن يكون أكبر من صفر / File size must be greater than zero")
                .LessThanOrEqualTo(50 * 1024 * 1024).WithMessage("حجم الملف يجب أن يكون أقل من 50 ميجابايت / File size must be less than 50MB");

            RuleFor(x => x.PageCount)
                .GreaterThan(0).WithMessage("عدد الصفحات يجب أن يكون أكبر من صفر / Page count must be greater than zero")
                .LessThanOrEqualTo(10000).WithMessage("عدد الصفحات مبالغ فيه / Page count is too high");

            RuleFor(x => x.ISBN)
                .Must(BeValidISBN).When(x => !string.IsNullOrEmpty(x.ISBN))
                .WithMessage("رقم ISBN غير صحيح / Invalid ISBN format");

            RuleFor(x => x.PublishedDate)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.PublishedDate.HasValue)
                .WithMessage("تاريخ النشر لا يمكن أن يكون في المستقبل / Published date cannot be in the future");
        }

        private bool BeValidPdfPath(string pdfPath)
        {
            if (string.IsNullOrEmpty(pdfPath))
                return false;

            return pdfPath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
        }

        private bool BeValidLanguage(string language)
        {
            var validLanguages = new[] { "ar", "en", "ur", "tr", "fr" };
            return validLanguages.Contains(language?.ToLower());
        }

        private bool BeValidISBN(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return true; // Optional field

            // Remove hyphens and spaces
            isbn = isbn.Replace("-", "").Replace(" ", "");

            // Check if it's ISBN-10 or ISBN-13
            if (isbn.Length == 10)
                return IsValidISBN10(isbn);
            else if (isbn.Length == 13)
                return IsValidISBN13(isbn);

            return false;
        }

        private bool IsValidISBN10(string isbn)
        {
            if (isbn.Length != 10)
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (10 - i);
            }

            char lastChar = isbn[9];
            if (lastChar == 'X')
                sum += 10;
            else if (char.IsDigit(lastChar))
                sum += (lastChar - '0');
            else
                return false;

            return sum % 11 == 0;
        }

        private bool IsValidISBN13(string isbn)
        {
            if (isbn.Length != 13)
                return false;

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
            }

            if (!char.IsDigit(isbn[12]))
                return false;

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit == (isbn[12] - '0');
        }
    }
    public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("عنوان الكتاب يجب أن يكون أقل من 200 حرف / Title must be less than 200 characters")
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("عنوان الكتاب يجب أن يكون أكثر من 3 أحرف / Title must be more than 3 characters");

            RuleFor(x => x.Author)
                .MaximumLength(100).WithMessage("اسم المؤلف يجب أن يكون أقل من 100 حرف / Author name must be less than 100 characters")
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Author))
                .WithMessage("اسم المؤلف يجب أن يكون أكثر من حرفين / Author name must be more than 2 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("الوصف يجب أن يكون أقل من 1000 حرف / Description must be less than 1000 characters");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue)
                .WithMessage("يجب اختيار فئة صحيحة / Valid category must be selected");

            RuleFor(x => x.Status)
                .IsInEnum().When(x => x.Status.HasValue)
                .WithMessage("حالة الكتاب غير صحيحة / Invalid book status");

            RuleFor(x => x.PublishedDate)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.PublishedDate.HasValue)
                .WithMessage("تاريخ النشر لا يمكن أن يكون في المستقبل / Published date cannot be in the future");
        }
    }
}
