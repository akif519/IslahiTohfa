using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;

namespace IslahiTohfa.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Book mappings
        CreateMap<Book, BookDto>()
            .ReverseMap();
            
        CreateMap<CreateBookDto, Book>()
            .ForMember(dest => dest.PdfFilePath, opt => opt.Ignore())
            .ForMember(dest => dest.CoverImagePath, opt => opt.Ignore())
            .ForMember(dest => dest.FileSize, opt => opt.Ignore());
            
        CreateMap<UpdateBookDto, Book>()
            .ForMember(dest => dest.PdfFilePath, opt => opt.Ignore())
            .ForMember(dest => dest.CoverImagePath, opt => opt.Ignore())
            .ForMember(dest => dest.FileSize, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
        
        // Comment mappings
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.User.FirstName) 
                    ? $"{src.User.FirstName} {src.User.LastName}".Trim() 
                    : src.User.UserName))
            .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicturePath));
            
        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        
        // User mappings
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}
