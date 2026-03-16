using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class PaymentRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IPaymentRepository
{
    public async Task<Guid> Insert(Payment payment)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        await ctx.Payments.AddAsync(payment);
        await ctx.SaveChangesAsync();
        return payment.Id;
    }
}