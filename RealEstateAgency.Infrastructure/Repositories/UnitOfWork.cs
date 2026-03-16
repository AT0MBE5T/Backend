using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class UnitOfWork(RealEstateContext context) : IUnitOfWork
{
    public async Task BeginTransactionAsync()
        => await context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
        => await context.Database.CommitTransactionAsync();

    public async Task RollbackAsync()
        => await context.Database.RollbackTransactionAsync();
}