using FluentValidation;
using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("محتوى التعليق مطلوب / Comment content is required")
                .MinimumLength(5).WithMessage("التعليق قصير جداً / Comment is too short")
                .MaximumLength(1000).WithMessage("التعليق طويل جداً / Comment is too long")
                .Must(NotContainInappropriateContent).WithMessage("التعليق يحتوي على محتوى غير مناسب / Comment contains inappropriate content");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("معرف الكتاب غير صحيح / Invalid book ID");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("معرف المستخدم غير صحيح / Invalid user ID");

            RuleFor(x => x.ParentCommentId)
                .GreaterThan(0).When(x => x.ParentCommentId.HasValue)
                .WithMessage("معرف التعليق الأصلي غير صحيح / Invalid parent comment ID");
        }

        private bool NotContainInappropriateContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return true;

            // List of inappropriate words/phrases (extend as needed)
            var inappropriateWords = new[]
            {
                "spam", "scam", "hack", "virus", "malware",
                // Add Arabic inappropriate words
                "احتيال", "فيروس", "اختراق"
            };

            var lowerContent = content.ToLower();
            return !inappropriateWords.Any(word => lowerContent.Contains(word.ToLower()));
        }
    }

    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("محتوى التعليق مطلوب / Comment content is required")
                .MinimumLength(5).WithMessage("التعليق قصير جداً / Comment is too short")
                .MaximumLength(1000).WithMessage("التعليق طويل جداً / Comment is too long")
                .Must(NotContainInappropriateContent).WithMessage("التعليق يحتوي على محتوى غير مناسب / Comment contains inappropriate content");
        }

        private bool NotContainInappropriateContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                return true;

            var inappropriateWords = new[]
            {
                "spam", "scam", "hack", "virus", "malware",
                "احتيال", "فيروس", "اختراق"
            };

            var lowerContent = content.ToLower();
            return !inappropriateWords.Any(word => lowerContent.Contains(word.ToLower()));
        }
    }
}
