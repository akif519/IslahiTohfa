using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public class TextAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var text = value.ToString();
            var arabicPattern = @"^[\u0600-\u06FF\s\d.,!?()]+$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(text, arabicPattern))
            {
                return new ValidationResult("النص يجب أن يحتوي على أحرف عربية فقط / Text must contain only Arabic characters");
            }

            return ValidationResult.Success;
        }
    }

    public class IslamicContentAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var content = value.ToString().ToLower();

            // List of prohibited content keywords
            var prohibitedKeywords = new[]
            {
                "gambling", "casino", "porn", "adult", "dating",
                "قمار", "مراهنة", "إباحي", "للكبار"
            };

            if (prohibitedKeywords.Any(keyword => content.Contains(keyword)))
            {
                return new ValidationResult("المحتوى غير مناسب للمنصة الإسلامية / Content is inappropriate for Islamic platform");
            }

            return ValidationResult.Success;
        }
    }
}
