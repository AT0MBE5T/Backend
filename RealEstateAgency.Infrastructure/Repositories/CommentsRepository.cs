using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class CommentsRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : ICommentsRepository
{
    public async Task<List<CommentDto>> GetAllByAnnouncementIdAsync(Guid id)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        return await ctx.Comments
            .Where(x => x.AnnouncementId == id)
            .Select(x => new CommentDto
            {
                Id = x.Id,
                AnnouncementId =  x.AnnouncementId,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                UserId =  x.UserId,
                AuthorName = $"{x.UserNavigation.Name} {x.UserNavigation.Surname}"
            })
            .OrderBy(x => x.CreatedAt)
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
    
    public async Task<List<CommentGrid>> GetCommentsGridAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var result = await ctx.Comments.Select(x => new CommentGrid
        {
            Id = x.Id,
            Author = x.UserNavigation.UserName,
            StatementTitle = x.AnnouncementNavigation.StatementNavigation.Title,
            Text = x.Text,
            CreatedAt = x.CreatedAt,
            AnnouncementId = x.AnnouncementId
        }).ToListAsync();

        return result;
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