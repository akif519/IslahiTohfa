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
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(l => l.Book)
                .WithMany(b => b.Likes)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Constraints
            builder.HasIndex(l => new { l.BookId, l.UserId }).IsUnique();
        }
    }
}
