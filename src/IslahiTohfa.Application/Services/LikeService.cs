using IslahiTohfa.Application.Interfaces;

namespace IslahiTohfa.Application.Services;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ToggleLikeAsync(int bookId, string userId)
    {
        var isLiked = await _unitOfWork.Likes.ToggleLikeAsync(bookId, userId);
        await _unitOfWork.SaveChangesAsync();
        return isLiked;
    }

    public async Task<bool> IsBookLikedByUserAsync(int bookId, string userId)
    {
        return await _unitOfWork.Books.IsBookLikedByUserAsync(bookId, userId);
    }

    public async Task<int> GetLikeCountForBookAsync(int bookId)
    {
        return await _unitOfWork.Likes.GetLikeCountForBookAsync(bookId);
    }
}
