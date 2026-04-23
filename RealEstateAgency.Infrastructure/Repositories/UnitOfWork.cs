using Microsoft.EntityFrameworkCore.Storage;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RealEstateContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(RealEstateContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) 
        => await _context.SaveChangesAsync(ct);

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

    public void Dispose() => _context.Dispose();
}