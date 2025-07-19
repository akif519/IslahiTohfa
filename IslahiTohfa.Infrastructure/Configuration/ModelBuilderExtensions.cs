using IslahiTohfa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Configuration
{
    // Additional configuration classes for complex scenarios
    public static class ModelBuilderExtensions
    {
        public static void ConfigureIdentityTables(this ModelBuilder modelBuilder)
        {
            // Configure table names and relationships for ASP.NET Core Identity if used
            // This would be used if integrating with ASP.NET Core Identity instead of custom User entity
        }

        public static void ConfigureIndexes(this ModelBuilder modelBuilder)
        {
            // Additional composite indexes for performance
            modelBuilder.Entity<Book>()
                .HasIndex(b => new { b.CategoryId, b.AverageRating, b.ViewCount })
                .HasDatabaseName("IX_Books_Category_Rating_Views");

            modelBuilder.Entity<UserActivity>()
                .HasIndex(a => new { a.UserId, a.ActivityType, a.ActivityDate })
                .HasDatabaseName("IX_UserActivities_User_Type_Date");

            // Full-text search index for book content (SQL Server specific)
            // This would require additional SQL scripts for full-text catalog setup
        }

        public static void ConfigureViews(this ModelBuilder modelBuilder)
        {
            // Configure database views for complex queries
            // Example: Popular books view
            modelBuilder.Entity<PopularBooksView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_PopularBooks");
                entity.Property(e => e.BookId).HasColumnName("BookId");
                entity.Property(e => e.Title).HasColumnName("Title");
                entity.Property(e => e.PopularityScore).HasColumnName("PopularityScore");
            });
        }
    }

    // View entities for complex queries
    public class PopularBooksView
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public double PopularityScore { get; set; }
    }
}
