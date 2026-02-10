namespace IslahiTohfa.Application.Interfaces;

public interface ILikeService
{
    Task<bool> ToggleLikeAsync(int bookId, string userId);
    Task<bool> IsBookLikedByUserAsync(int bookId, string userId);
    Task<int> GetLikeCountForBookAsync(int bookId);
}
