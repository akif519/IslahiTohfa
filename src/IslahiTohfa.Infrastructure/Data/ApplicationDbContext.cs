using IslahiTohfa.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IslahiTohfa.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Book Configuration
        builder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.ISBN).HasMaxLength(20);
            entity.Property(e => e.Publisher).HasMaxLength(150);
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.PdfFilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CoverImagePath).HasMaxLength(500);
            
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.IsFeatured);
            entity.HasIndex(e => e.IsDeleted);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasMany(e => e.Likes)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Comment Configuration
        builder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.ModeratorNotes).HasMaxLength(500);
            
            entity.HasIndex(e => e.BookId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.IsDeleted);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Like Configuration
        builder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasIndex(e => new { e.BookId, e.UserId }).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsDeleted);
            
            entity.HasQueryFilter(e => !e.IsDeleted);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ApplicationUser Configuration
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.ProfilePicturePath).HasMaxLength(500);
            entity.Property(e => e.Bio).HasMaxLength(500);
        });
    }
}
