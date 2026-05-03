using Microsoft.EntityFrameworkCore.Storage;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public readonly RealEstateContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(RealEstateContext context) => _context = context;

    public async Task BeginTransactionAsync() 
        => _currentTransaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction!.CommitAsync();
        }
        finally
        {
            _currentTransaction?.Dispose();
        }
    }

    public async Task RollbackAsync()
    {
        await _currentTransaction!.RollbackAsync();
        _currentTransaction.Dispose();
    }
}