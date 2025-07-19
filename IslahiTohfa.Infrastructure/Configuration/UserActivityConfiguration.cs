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
    public class UserActivityConfiguration : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            builder.ToTable("UserActivities");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.ActivityType)
                .HasConversion<int>();

            builder.Property(a => a.ActivityDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.AdditionalData)
                .HasMaxLength(1000);

            // Relationships
            builder.HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Book)
                .WithMany(b => b.UserActivities)
                .HasForeignKey(a => a.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.BookId);
            builder.HasIndex(a => a.ActivityType);
            builder.HasIndex(a => a.ActivityDate);
        }
    }
}
