using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage("البريد الإلكتروني أو اسم المستخدم مطلوب / Email or username is required")
                .MaximumLength(100).WithMessage("البريد الإلكتروني أو اسم المستخدم طويل جداً / Email or username is too long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة / Password is required")
                .MinimumLength(6).WithMessage("كلمة المرور قصيرة جداً / Password is too short");
        }
    }
}
