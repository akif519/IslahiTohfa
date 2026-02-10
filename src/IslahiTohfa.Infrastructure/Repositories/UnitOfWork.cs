using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace IslahiTohfa.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public IBookRepository Books { get; }
    public ICommentRepository Comments { get; }
    public ILikeRepository Likes { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IBookRepository bookRepository,
        ICommentRepository commentRepository,
        ILikeRepository likeRepository)
    {
        _context = context;
        Books = bookRepository;
        Comments = commentRepository;
        Likes = likeRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
