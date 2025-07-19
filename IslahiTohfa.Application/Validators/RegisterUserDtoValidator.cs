using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب / Email is required")
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح / Invalid email format")
                .MaximumLength(100).WithMessage("البريد الإلكتروني طويل جداً / Email is too long");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب / Username is required")
                .MinimumLength(3).WithMessage("اسم المستخدم يجب أن يكون 3 أحرف على الأقل / Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("اسم المستخدم طويل جداً / Username is too long")
                .Matches("^[a-zA-Z0-9_-]+$").WithMessage("اسم المستخدم يجب أن يحتوي على أحرف وأرقام فقط / Username can only contain letters, numbers, underscore, and dash");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة / Password is required")
                .MinimumLength(6).WithMessage("كلمة المرور يجب أن تكون 6 أحرف على الأقل / Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("كلمة المرور طويلة جداً / Password is too long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم / Password must contain uppercase, lowercase, and number");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("تأكيد كلمة المرور مطلوب / Password confirmation is required")
                .Equal(x => x.Password).WithMessage("كلمة المرور وتأكيدها غير متطابقين / Passwords do not match");

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("الاسم الأول طويل جداً / First name is too long")
                .Matches(@"^[\u0600-\u06FFa-zA-Z\s]+$").When(x => !string.IsNullOrEmpty(x.FirstName))
                .WithMessage("الاسم الأول يجب أن يحتوي على أحرف فقط / First name can only contain letters");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("اسم العائلة طويل جداً / Last name is too long")
                .Matches(@"^[\u0600-\u06FFa-zA-Z\s]+$").When(x => !string.IsNullOrEmpty(x.LastName))
                .WithMessage("اسم العائلة يجب أن يحتوي على أحرف فقط / Last name can only contain letters");

            RuleFor(x => x.PreferredLanguage)
                .Must(BeValidLanguage).WithMessage("اللغة المفضلة غير صحيحة / Invalid preferred language");
        }

        private bool BeValidLanguage(string language)
        {
            var validLanguages = new[] { "ar", "en", "ur" };
            return validLanguages.Contains(language?.ToLower());
        }
    }
}
