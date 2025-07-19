using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using IslahiTohfa.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new LikeConfiguration());
            modelBuilder.ApplyConfiguration(new UserActivityConfiguration());
            modelBuilder.ApplyConfiguration(new BookmarkConfiguration());

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Islamic Theology", NameArabic = "العقيدة الإسلامية", Description = "Books about Islamic faith and beliefs", IconClass = "fas fa-mosque", SortOrder = 1, IsActive = true },
                new Category { Id = 2, Name = "Quran & Tafsir", NameArabic = "القرآن والتفسير", Description = "Quranic studies and interpretations", IconClass = "fas fa-book-open", SortOrder = 2, IsActive = true },
                new Category { Id = 3, Name = "Hadith", NameArabic = "الحديث الشريف", Description = "Prophetic traditions and their studies", IconClass = "fas fa-scroll", SortOrder = 3, IsActive = true },
                new Category { Id = 4, Name = "Islamic Jurisprudence", NameArabic = "الفقه الإسلامي", Description = "Islamic law and jurisprudence", IconClass = "fas fa-balance-scale", SortOrder = 4, IsActive = true },
                new Category { Id = 5, Name = "Islamic History", NameArabic = "التاريخ الإسلامي", Description = "History of Islam and Muslims", IconClass = "fas fa-landmark", SortOrder = 5, IsActive = true },
                new Category { Id = 6, Name = "Islamic Ethics", NameArabic = "الأخلاق الإسلامية", Description = "Islamic morality and character building", IconClass = "fas fa-heart", SortOrder = 6, IsActive = true },
                new Category { Id = 7, Name = "Spirituality", NameArabic = "التزكية والروحانية", Description = "Islamic spirituality and purification", IconClass = "fas fa-hands-praying", SortOrder = 7, IsActive = true },
                new Category { Id = 8, Name = "Contemporary Issues", NameArabic = "القضايا المعاصرة", Description = "Modern Islamic perspectives on current issues", IconClass = "fas fa-globe", SortOrder = 8, IsActive = true }
            );

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@islahitohfa.com",
                    UserName = "admin",
                    FirstName = "System",
                    LastName = "Administrator",
                    PasswordHash = "$2a$11$rQZOJ1BHoNTWDzJzJzJzJe4pGOJ1BHoNTWDzJzJzJe4pGOJ1BHoNT", // "Admin@123"
                    Role = UserRole.Admin,
                    CreatedDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    IsActive = true,
                    EmailConfirmed = true,
                    PreferredLanguage = "ar"
                }
            );
        }
    }
}
