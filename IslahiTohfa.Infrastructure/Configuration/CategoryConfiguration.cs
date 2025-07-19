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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.NameArabic)
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.IconClass)
                .HasMaxLength(50);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasIndex(c => c.SortOrder);
        }
    }
}
