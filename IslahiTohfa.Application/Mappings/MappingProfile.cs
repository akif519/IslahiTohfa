using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateBookMappings();
            CreateUserMappings();
            CreateCategoryMappings();
            CreateCommentMappings();
            CreateRatingMappings();
            CreateLikeMappings();
            CreateActivityMappings();
        }

        private void CreateBookMappings()
        {
            // Book -> BookDto
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryNameArabic, opt => opt.MapFrom(src => src.Category.NameArabic))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count(c => c.IsActive)))
                .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.CurrentUserRating, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // Implement if needed
                .ForMember(dest => dest.RelatedBooks, opt => opt.Ignore()); // Implement if needed

            // CreateBookDto -> Book
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.DownloadCount, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRatings, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLikes, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.UserActivities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore());

            // UpdateBookDto -> Book
            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
                .ForMember(dest => dest.DownloadCount, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRatings, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLikes, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.UserActivities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore());

            // BookSummaryDto for lighter operations
            CreateMap<Book, BookSummaryDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }

        private void CreateUserMappings()
        {
            // User -> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.TotalBooks, opt => opt.Ignore()) // Calculate in service if needed
                .ForMember(dest => dest.TotalComments, opt => opt.MapFrom(src => src.Comments.Count(c => c.IsActive)))
                .ForMember(dest => dest.TotalRatings, opt => opt.MapFrom(src => src.Ratings.Count()));

            // RegisterUserDto -> User
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Set in service
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore());

            // UpdateUserDto -> User
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore()) // Email updates require verification
                .ForMember(dest => dest.UserName, opt => opt.Ignore()) // Username updates might be restricted
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore()) // Role updates require admin privileges
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Ratings, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.Activities, opt => opt.Ignore())
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore());
        }

        private void CreateCategoryMappings()
        {
            // Category -> CategoryDto
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src =>
                    src.Books.Count(b => b.IsActive && b.Status == BookStatus.Published)));

            // CreateCategoryDto -> Category
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            // UpdateCategoryDto -> Category
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }

        private void CreateCommentMappings()
        {
            // Comment -> CommentDto
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}".Trim()))
                .ForMember(dest => dest.UserProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.RepliesCount, opt => opt.MapFrom(src => src.Replies.Count(r => r.IsActive)))
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore()) // Set in service based on current user
                .ForMember(dest => dest.CanDelete, opt => opt.Ignore()); // Set in service based on current user

            // CreateCommentDto -> Comment
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore());

            // UpdateCommentDto -> Comment
            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCommentId, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ParentComment, opt => opt.Ignore())
                .ForMember(dest => dest.Replies, opt => opt.Ignore());
        }

        private void CreateRatingMappings()
        {
            // Rating -> RatingDto
            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

            // CreateRatingDto -> Rating
            CreateMap<CreateRatingDto, Rating>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }

        private void CreateLikeMappings()
        {
            // Like -> LikeDto
            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

            // CreateLikeDto -> Like
            CreateMap<CreateLikeDto, Like>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }

        private void CreateActivityMappings()
        {
            // UserActivity -> UserActivityDto
            CreateMap<UserActivity, UserActivityDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.ToString()));

            // UserActivityDto -> UserActivity
            CreateMap<UserActivityDto, UserActivity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ActivityDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore());

            // Create analytics mappings
            CreateMap<UserActivity, AnalyticsDataDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ActivityDate.Date))
                .ForMember(dest => dest.Count, opt => opt.Ignore()) // Will be set during aggregation
                .ForMember(dest => dest.Value, opt => opt.Ignore()); // Will be set during aggregation
        }
    }
}
