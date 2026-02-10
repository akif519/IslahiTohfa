namespace IslahiTohfa.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    ICommentRepository Comments { get; }
    ILikeRepository Likes { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
