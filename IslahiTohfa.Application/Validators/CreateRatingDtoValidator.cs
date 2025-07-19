using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class CreateRatingDtoValidator : AbstractValidator<CreateRatingDto>
    {
        public CreateRatingDtoValidator()
        {
            RuleFor(x => x.Value)
                .InclusiveBetween(1, 5).WithMessage("التقييم يجب أن يكون بين 1 و 5 / Rating must be between 1 and 5");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("معرف الكتاب غير صحيح / Invalid book ID");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("معرف المستخدم غير صحيح / Invalid user ID");
        }
    }
}
