using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;
using RealEstateAgency.Infrastructure.Contexts;

namespace RealEstateAgency.Infrastructure.Repositories;

public class CommentRepository(RealEstateContext ctx) : ICommentRepository
{
    public async Task<List<CommentDto>> GetAllByAnnouncementIdAsync(Guid id)
    {
        var result = await ctx.Comments
            .AsNoTracking()
            .Where(x => x.AnnouncementId == id)
            .Select(x => new CommentDto
            {
                Id = x.Id,
                AnnouncementId =  x.AnnouncementId,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                UserId =  x.UserId,
                AuthorName = $"{x.UserNavigation!.Name} {x.UserNavigation.Surname}"
            })
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
        return result;
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var comment = await ctx.Comments.FindAsync(id);
        if (comment == null) return false;

        ctx.Comments.Remove(comment);
        return true; 
    }

    public async Task<Comment?> GetCommentByIdAsync(Guid id)
    {
        var result = await ctx.Comments
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return result;
    }
    
    public async Task<List<CommentGridDto>> GetCommentsGridAsync()
    {
        var result = await ctx.Comments
            .Select(x => new CommentGridDto
            {
                Id = x.Id,
                Author = x.UserNavigation!.UserName!,
                StatementTitle = x.AnnouncementNavigation!.StatementNavigation!.Title,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                AnnouncementId = x.AnnouncementId
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return result;
    }
    
    public async Task<Guid> InsertAsync(Comment comment)
    {
        try
        {
            await ctx.Comments
                .AddAsync(comment);
            return comment.Id;
        }
        catch
        {
            return Guid.Empty;
        }
    }
}