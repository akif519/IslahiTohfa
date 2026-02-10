using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByBookIdAsync(int bookId)
    {
        var comments = await _unitOfWork.Comments.GetCommentsByBookIdAsync(bookId);
        return _mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createDto, string userId)
    {
        var comment = _mapper.Map<Comment>(createDto);
        comment.UserId = userId;
        comment.Status = CommentStatus.Pending; // Auto-moderation
        
        await _unitOfWork.Comments.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<bool> DeleteCommentAsync(int id, string userId)
    {
        var comment = await _unitOfWork.Comments.GetByIdAsync(id);
        if (comment == null || comment.UserId != userId)
            return false;
        
        comment.IsDeleted = true;
        _unitOfWork.Comments.Update(comment);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }

    public async Task<PaginatedList<CommentDto>> GetCommentsByStatusAsync(CommentStatus status, int pageNumber = 1, int pageSize = 20)
    {
        var comments = await _unitOfWork.Comments.GetCommentsByStatusAsync(status, pageNumber, pageSize);
        
        return new PaginatedList<CommentDto>
        {
            Items = _mapper.Map<List<CommentDto>>(comments.Items),
            PageNumber = comments.PageNumber,
            PageSize = comments.PageSize,
            TotalCount = comments.TotalCount
        };
    }

    public async Task ApproveCommentAsync(int commentId, string moderatorId, string? notes = null)
    {
        await _unitOfWork.Comments.ApproveCommentAsync(commentId, moderatorId, notes);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RejectCommentAsync(int commentId, string moderatorId, string notes)
    {
        await _unitOfWork.Comments.RejectCommentAsync(commentId, moderatorId, notes);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<double> GetAverageRatingForBookAsync(int bookId)
    {
        return await _unitOfWork.Comments.GetAverageRatingForBookAsync(bookId);
    }
}
