using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Mappings
{
    public static class MappingExtensions
    {
        public static BookDto ToBookDtoWithUserContext(this Book book, int? currentUserId, IMapper mapper)
        {
            var dto = mapper.Map<BookDto>(book);

            if (currentUserId.HasValue)
            {
                dto.IsLikedByCurrentUser = book.Likes.Any(l => l.UserId == currentUserId.Value);
                var userRating = book.Ratings.FirstOrDefault(r => r.UserId == currentUserId.Value);
                dto.CurrentUserRating = userRating?.Value;
            }

            return dto;
        }

        public static CommentDto ToCommentDtoWithUserContext(this Comment comment, int? currentUserId, IMapper mapper)
        {
            var dto = mapper.Map<CommentDto>(comment);

            if (currentUserId.HasValue)
            {
                dto.CanEdit = comment.UserId == currentUserId.Value;
                dto.CanDelete = comment.UserId == currentUserId.Value; // Or admin check
            }

            return dto;
        }

        public static IQueryable<BookDto> ProjectToBookDto(this IQueryable<Book> books)
        {
            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                ThumbnailUrl = b.ThumbnailUrl,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CategoryNameArabic = b.Category.NameArabic,
                Status = b.Status,
                PublishedDate = b.PublishedDate,
                ViewCount = b.ViewCount,
                DownloadCount = b.DownloadCount,
                AverageRating = b.AverageRating,
                TotalRatings = b.TotalRatings,
                TotalLikes = b.TotalLikes,
                Language = b.Language,
                FileSizeBytes = b.FileSizeBytes,
                PageCount = b.PageCount,
                ISBN = b.ISBN,
                IsFeatured = b.IsFeatured,
                CommentsCount = b.Comments.Count(c => c.IsActive)
            });
        }
    }
}
