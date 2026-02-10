using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Domain.Entities;
using IslahiTohfa.Domain.Enums;

namespace IslahiTohfa.Application.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByBookIdAsync(int bookId);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(string userId);
    Task<IEnumerable<Comment>> GetPendingCommentsAsync();
    Task<PaginatedList<Comment>> GetCommentsByStatusAsync(CommentStatus status, int pageNumber = 1, int pageSize = 20);
    Task<double> GetAverageRatingForBookAsync(int bookId);
    Task ApproveCommentAsync(int commentId, string moderatorId, string? notes = null);
    Task RejectCommentAsync(int commentId, string moderatorId, string notes);
}
