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
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Value)
                .IsRequired();

            builder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            builder.HasOne(r => r.Book)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Constraints
            builder.HasIndex(r => new { r.BookId, r.UserId }).IsUnique();
            builder.HasCheckConstraint("CK_Rating_Value", "Value >= 1 AND Value <= 5");
        }
    }
}
