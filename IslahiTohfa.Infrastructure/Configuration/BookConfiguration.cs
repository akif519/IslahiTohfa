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
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Description)
                .HasMaxLength(1000);

            builder.Property(b => b.ThumbnailUrl)
                .HasMaxLength(200);

            builder.Property(b => b.PdfFilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.Status)
                .HasConversion<int>();

            builder.Property(b => b.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.UpdatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.ViewCount)
                .HasDefaultValue(0);

            builder.Property(b => b.DownloadCount)
                .HasDefaultValue(0);

            builder.Property(b => b.AverageRating)
                .HasDefaultValue(0)
                .HasColumnType("decimal(3,2)");

            builder.Property(b => b.TotalRatings)
                .HasDefaultValue(0);

            builder.Property(b => b.TotalLikes)
                .HasDefaultValue(0);

            builder.Property(b => b.Language)
                .HasMaxLength(5)
                .HasDefaultValue("ar");

            builder.Property(b => b.ISBN)
                .HasMaxLength(50);

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);

            builder.Property(b => b.IsFeatured)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(b => b.Title);
            builder.HasIndex(b => b.Author);
            builder.HasIndex(b => b.CategoryId);
            builder.HasIndex(b => b.Status);
            builder.HasIndex(b => b.AverageRating);
            builder.HasIndex(b => b.ViewCount);
            builder.HasIndex(b => b.IsFeatured);
            builder.HasIndex(b => new { b.IsActive, b.Status });
        }
    }
}
