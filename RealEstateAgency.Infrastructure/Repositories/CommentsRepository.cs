using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class CommentsRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : ICommentsRepository
{
    public async Task<List<Comment>> GetAllByAnnouncementIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Comments
            .Where(x => x.AnnouncementId == id)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            var res = await ctx.Comments
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            return res != 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Comment?> GetCommentByIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Comments
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<Guid> InsertAsync(Comment comment)
    {
        try
        {
            await using var ctx = await dbContextFactory.CreateDbContextAsync();
            await ctx.Comments
                .AddAsync(comment);
            await ctx.SaveChangesAsync();
            return comment.Id;
        }
        catch
        {
            return Guid.Empty;
        }
    }
}