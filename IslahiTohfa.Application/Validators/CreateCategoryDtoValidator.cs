using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الفئة مطلوب / Category name is required")
                .MaximumLength(100).WithMessage("اسم الفئة طويل جداً / Category name is too long")
                .MinimumLength(2).WithMessage("اسم الفئة قصير جداً / Category name is too short");

            RuleFor(x => x.NameArabic)
                .MaximumLength(100).WithMessage("الاسم العربي طويل جداً / Arabic name is too long")
                .Matches(@"^[\u0600-\u06FF\s]+$").When(x => !string.IsNullOrEmpty(x.NameArabic))
                .WithMessage("الاسم العربي يجب أن يحتوي على أحرف عربية فقط / Arabic name can only contain Arabic letters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("الوصف طويل جداً / Description is too long");

            RuleFor(x => x.IconClass)
                .MaximumLength(50).WithMessage("كلاس الأيقونة طويل جداً / Icon class is too long")
                .Matches(@"^[a-zA-Z0-9\s\-_]+$").When(x => !string.IsNullOrEmpty(x.IconClass))
                .WithMessage("كلاس الأيقونة غير صحيح / Invalid icon class format");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("ترتيب الفئة يجب أن يكون صفر أو أكثر / Sort order must be zero or greater");
        }
    }
}
