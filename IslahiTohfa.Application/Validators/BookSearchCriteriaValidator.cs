using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class BookSearchCriteriaValidator : AbstractValidator<BookSearchCriteria>
    {
        public BookSearchCriteriaValidator()
        {
            RuleFor(x => x.Query)
                .MaximumLength(200).WithMessage("استعلام البحث طويل جداً / Search query is too long");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).When(x => x.CategoryId.HasValue)
                .WithMessage("معرف الفئة غير صحيح / Invalid category ID");

            RuleFor(x => x.Author)
                .MaximumLength(100).WithMessage("اسم المؤلف طويل جداً / Author name is too long");

            RuleFor(x => x.Language)
                .Must(BeValidLanguage).When(x => !string.IsNullOrEmpty(x.Language))
                .WithMessage("كود اللغة غير صحيح / Invalid language code");

            RuleFor(x => x.MinRating)
                .InclusiveBetween(0, 5).When(x => x.MinRating.HasValue)
                .WithMessage("الحد الأدنى للتقييم يجب أن يكون بين 0 و 5 / Minimum rating must be between 0 and 5");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("رقم الصفحة يجب أن يكون أكبر من صفر / Page number must be greater than zero");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("حجم الصفحة يجب أن يكون بين 1 و 100 / Page size must be between 1 and 100");

            RuleFor(x => x.SortBy)
                .IsInEnum().WithMessage("طريقة الترتيب غير صحيحة / Invalid sort order");

            RuleFor(x => x.SortDirection)
                .IsInEnum().WithMessage("اتجاه الترتيب غير صحيح / Invalid sort direction");
        }

        private bool BeValidLanguage(string language)
        {
            var validLanguages = new[] { "ar", "en", "ur", "tr", "fr" };
            return validLanguages.Contains(language?.ToLower());
        }
    }
}
