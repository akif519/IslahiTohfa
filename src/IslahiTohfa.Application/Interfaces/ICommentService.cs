using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsByBookIdAsync(int bookId);
    Task<CommentDto> CreateCommentAsync(CreateCommentDto createDto, string userId);
    Task<bool> DeleteCommentAsync(int id, string userId);
    Task<PaginatedList<CommentDto>> GetCommentsByStatusAsync(CommentStatus status, int pageNumber = 1, int pageSize = 20);
    Task ApproveCommentAsync(int commentId, string moderatorId, string? notes = null);
    Task RejectCommentAsync(int commentId, string moderatorId, string notes);
    Task<double> GetAverageRatingForBookAsync(int bookId);
}
