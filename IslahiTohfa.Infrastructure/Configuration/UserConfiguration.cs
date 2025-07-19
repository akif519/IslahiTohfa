using IslahiTohfa.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasMaxLength(50);

            builder.Property(u => u.Role)
                .HasConversion<int>();

            builder.Property(u => u.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.EmailConfirmed)
                .HasDefaultValue(false);

            builder.Property(u => u.PreferredLanguage)
                .HasMaxLength(5)
                .HasDefaultValue("ar");

            // Indexes
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Role);
        }
    }
}
