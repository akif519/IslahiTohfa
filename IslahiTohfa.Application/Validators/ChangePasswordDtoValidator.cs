using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("كلمة المرور الحالية مطلوبة / Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("كلمة المرور الجديدة مطلوبة / New password is required")
                .MinimumLength(6).WithMessage("كلمة المرور الجديدة يجب أن تكون 6 أحرف على الأقل / New password must be at least 6 characters")
                .MaximumLength(100).WithMessage("كلمة المرور الجديدة طويلة جداً / New password is too long")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم / Password must contain uppercase, lowercase, and number")
                .NotEqual(x => x.CurrentPassword).WithMessage("كلمة المرور الجديدة يجب أن تكون مختلفة عن الحالية / New password must be different from current password");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("تأكيد كلمة المرور الجديدة مطلوب / New password confirmation is required")
                .Equal(x => x.NewPassword).WithMessage("كلمة المرور الجديدة وتأكيدها غير متطابقين / New passwords do not match");
        }
    }
}
