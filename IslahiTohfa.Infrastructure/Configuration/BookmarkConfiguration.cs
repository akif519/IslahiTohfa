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
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable("Bookmarks");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.PageNumber)
                .IsRequired();

            builder.Property(b => b.Note)
                .HasMaxLength(500);

            builder.Property(b => b.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Book)
                .WithMany(book => book.Bookmarks)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(b => b.BookId);
            builder.HasIndex(b => new { b.UserId, b.BookId, b.PageNumber });
        }
    }
}

