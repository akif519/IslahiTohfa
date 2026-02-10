using IslahiTohfa.Domain.Entities;

namespace IslahiTohfa.Application.Interfaces;

public interface ILikeRepository : IRepository<Like>
{
    Task<Like?> GetLikeAsync(int bookId, string userId);
    Task<bool> ToggleLikeAsync(int bookId, string userId);
    Task<int> GetLikeCountForBookAsync(int bookId);
    Task<IEnumerable<Like>> GetUserLikesAsync(string userId);
}
