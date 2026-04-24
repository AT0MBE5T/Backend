using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class PaymentRepository(RealEstateContext ctx) : IPaymentRepository
{
    public async Task<Guid> Insert(Payment payment)
    {
        await ctx.Payments.AddAsync(payment);
        return payment.Id;
    }

    public async Task<bool> IsExistByAnnouncementId(Guid announcementId)
    {
        var result = await ctx.Payments.AnyAsync(p => p.AnnouncementId == announcementId);
        return result;
    }
}