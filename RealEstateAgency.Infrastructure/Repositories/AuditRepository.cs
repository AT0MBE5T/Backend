using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AuditRepository(RealEstateContext ctx) : IAuditRepository
{
    public async Task<Guid> InsertAsync(AuHistory record)
    {
        await ctx.AuHistories.AddAsync(record);
        return record.Id;
    }
    
    public async Task<List<AuditGridDto>> GetAll()
    {
        var res = await ctx.AuHistories
            .Select(x => new AuditGridDto
            {
                Id =  x.Id,
                CreatedAt = x.CreatedAt,
                ActionId =  x.ActionId,
                UserId =  x.UserId,
                Details =  x.Details,
                ActionName = x.ActionNavigation!.Name,
                UserName = x.UserNavigation!.UserName!
            }).ToListAsync();
        return res;
    }
}