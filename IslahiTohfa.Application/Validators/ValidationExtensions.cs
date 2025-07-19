using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeArabicText<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[\u0600-\u06FF\s\d.,!?()]+$")
                             .WithMessage("النص يجب أن يحتوي على أحرف عربية فقط / Text must contain only Arabic characters");
        }

        public static IRuleBuilderOptions<T, string> MustBeIslamicContent<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(content =>
            {
                if (string.IsNullOrEmpty(content))
                    return true;

                var lowerContent = content.ToLower();
                var prohibitedKeywords = new[]
                {
                    "gambling", "casino", "porn", "adult", "dating",
                    "قمار", "مراهنة", "إباحي", "للكبار"
                };

                return !prohibitedKeywords.Any(keyword => lowerContent.Contains(keyword));
            }).WithMessage("المحتوى غير مناسب للمنصة الإسلامية / Content is inappropriate for Islamic platform");
        }

        public static IRuleBuilderOptions<T, string> MustBeValidArabicName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[\u0600-\u06FF\s]{2,}$")
                             .WithMessage("الاسم يجب أن يحتوي على أحرف عربية فقط وأن يكون أكثر من حرفين / Name must contain only Arabic characters and be more than 2 characters");
        }

        public static IRuleBuilderOptions<T, string> MustBeStrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                             .WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير وصغير ورقم ورمز خاص / Password must contain uppercase, lowercase, number, and special character");
        }
    }
}
