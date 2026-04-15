using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;
using RealEstateAgency.Infrastructure.Context;

namespace RealEstateAgency.Infrastructure.Repositories;

public class AccountRepository(IDbContextFactory<RealEstateContext> dbContextFactory) : IAccountRepository
{
    public async Task<User?> GetUserById(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var user = await ctx.Users
            .Where(x => x.Id == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        
        return user;
    }
    
    public async Task<int> GetPlacedPropertyCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt == null)
            .CountAsync();
        
        return query;
    }
    
    public async Task<int> GetPlacedPropertyCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt == null
                && x.PublishedAt >= start
                &&  x.PublishedAt < end)
            .CountAsync();
        return query;
    }
    
    public async Task<int> GetPlacedPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt == null && x.PublishedAt >= start
                && x.PublishedAt < end)
            .CountAsync();
        
        return query;
    }
    
    public async Task<int> GetViewsDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var query = await ctx.Views
            .Where(x =>
                x.UserId == userId
                && x.CreatedAt >= start
                && x.CreatedAt < end)
            .CountAsync();
        
        return query;
    }
    
    public async Task<int> GetViewsDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var query = await ctx.Views
            .Where(x =>
                x.UserId == userId
                && x.CreatedAt >= start
                && x.CreatedAt < end)
            .CountAsync();
        
        return query;
    }
    
    public async Task<string> GetFavoriteCategoryDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().AddDays(1).Date;
        var end = start.AddDays(1);
        
        var query = await ctx.Views
            .Where(x => x.UserId == userId 
                        && x.CreatedAt >= start 
                        && x.CreatedAt < end)
            .GroupBy(x => x.AnnouncementNavigation.StatementNavigation.PropertyNavigation.PropertyTypeNavigation.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();
        
        return query ?? "Unknown";
    }
    
    public async Task<string> GetFavoriteCategoryDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var query = await ctx.Views
            .Where(x => x.UserId == userId 
                        && x.CreatedAt >= start 
                        && x.CreatedAt < end)
            .GroupBy(x => x.AnnouncementNavigation.StatementNavigation.PropertyNavigation.PropertyTypeNavigation.Name)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();
        
        return query ?? "Unknown";
    }
    
    public async Task<int> GetSoldPropertyCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt != null)
            .CountAsync();
        
        return query;
    }
    
    public async Task<int> GetSoldPropertyCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt != null
                && x.ClosedAt.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        return query;
    }
    
    public async Task<int> GetSoldPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.StatementNavigation!.UserId == userId
                && x.ClosedAt != null
                && x.ClosedAt.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        return query;
    }
    
    public async Task<int> GetBoughtPropertyCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var query = await ctx.Announcements
            .Where(x =>
                x.PaymentNavigation!.CustomerId == userId
                && x.ClosedAt != null)
            .CountAsync();
        return query;
    }
    
    public async Task<int> GetBoughtPropertyCntByUserIdDate(Guid userId,  DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.PaymentNavigation!.CustomerId == userId
                && x.ClosedAt != null
                && x.ClosedAt.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        return query;
    }
    
    public async Task<int> GetBoughtPropertyCntByUserIdDateSpan(Guid userId,  DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var query = await ctx.Announcements
            .Where(x =>
                x.PaymentNavigation!.CustomerId == userId
                && x.ClosedAt != null
                && x.ClosedAt.Value.Date >= start
                && x.ClosedAt.Value.Date < end)
            .CountAsync();
        
        return query;
    }

    public async Task<int> GetDaysFromRegisterByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var date = await ctx.Users
            .Where(x => x.Id == userId)
            .Select(x => DateTime.UtcNow - x.CreatedAt)
            .FirstOrDefaultAsync();
        
        return date.Days;
    }
    
    public async Task<int> GetPaymentsCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Payments
            .Where(x => x.CustomerId == userId)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetPaymentsCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Payments
            .Where(x =>
                x.CustomerId == userId
                && x.AnnouncementNavigation!.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end )
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetPaymentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res = await ctx.Payments
            .Where(x =>
                x.CustomerId == userId
                && x.AnnouncementNavigation!.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetQuestionsCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Comments
            .Where(x => x.UserId == userId)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetQuestionsCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Comments
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetQuestionsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res = await ctx.Comments
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();
        
        return res;
    }
    
    public async Task<int> GetAnswersCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Answers
            .Where(x => x.UserId == userId)
            .CountAsync();

        return res;
    }
    
    public async Task<bool> UpdateAsync(User user)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Users.FindAsync(user.Id);

        if (res is null)
            return false;

        res.PublicAvatarId = user.PublicAvatarId;
        res.Avatar =  user.Avatar;
        
        await ctx.SaveChangesAsync();
        return true;
    }
    
    // public async Task<User?> GetByIdAsync(Guid id)
    // {
    //     await using var ctx = await dbContextFactory.CreateDbContextAsync();
    //     var res = await ctx.Users.FindAsync(id);
    //
    //     return res;
    // }
    
    public async Task<int> GetAnswersCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Answers
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetAnswersCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res = await ctx.Answers
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetCommentsCntByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res = await ctx.Comments
            .Where(x => x.UserId == userId)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetCommentsCntByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Comments
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<int> GetCommentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res = await ctx.Comments
            .Where(x => x.UserId == userId && x.CreatedAt.Date >= start && x.CreatedAt.Date < end)
            .CountAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneyEarnedByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res= await ctx.Payments
            .Where(x => x.AnnouncementNavigation!.StatementNavigation!.UserId == userId)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneySpentByUserId(Guid userId)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res= await ctx.Payments
            .Where(x => x.CustomerId == userId)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneySpentByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Payments
            .Where(x =>
                x.CustomerId == userId
                && x.AnnouncementNavigation!.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneyEarnedByUserIdDate(Guid userId, DateTime dateTime)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTime.ToUniversalTime().Date.AddDays(1);
        var end = start.AddDays(1);
        
        var res = await ctx.Payments
            .Where(x =>
                        x.AnnouncementNavigation!.StatementNavigation!.UserId == userId
                        && x.AnnouncementNavigation.ClosedAt!.Value.Date >= start
                        && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneySpentByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res = await ctx.Payments
            .Where(x =>
                x.CustomerId == userId
                && x.AnnouncementNavigation!.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }
    
    public async Task<decimal> GetTotalMoneyEarnedByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        
        var start = dateTimeFrom.ToUniversalTime().Date.AddDays(1);
        var end = dateTimeTo.ToUniversalTime().Date.AddDays(2);
        
        var res= await ctx.Payments
            .Where(x =>
                x.AnnouncementNavigation!.StatementNavigation!.UserId == userId
                && x.AnnouncementNavigation.ClosedAt!.Value.Date >= start
                && x.AnnouncementNavigation.ClosedAt.Value.Date < end)
            .Select(x => x.AnnouncementNavigation!.StatementNavigation!.Price)
            .SumAsync();

        return res;
    }

    public async Task<Guid> GetUserIdByLogin(string login)
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();
        var res= await ctx.Users.Where(x => x.UserName == login)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        return res;
    }
    
    public async Task<List<UserGrid>> GetAllAsync()
    {
        await using var ctx = await dbContextFactory.CreateDbContextAsync();

        var res = await ctx.Users
            .Select(u => new UserGrid
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email ?? string.Empty,
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                Login = u.UserName ?? string.Empty,
                Age = u.Age,
                CreatedAt = u.CreatedAt,
                LockoutEnd = u.LockoutEnd,

                RoleId = ctx.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Select(ur => ur.RoleId)
                    .FirstOrDefault(),

                RoleName = ctx.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(ctx.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .FirstOrDefault() ?? "No Role",

                TotalOffersPlacedCnt = ctx.Announcements
                    .Count(a => a.StatementNavigation.UserId == u.Id),

                OffersClosedCnt = ctx.Announcements
                    .Count(a => a.StatementNavigation.UserId == u.Id && 
                                (a.ClosedAt != null || a.PaymentNavigation != null)),

                TotalRevenue = ctx.Announcements
                    .Where(a => a.StatementNavigation.UserId == u.Id && a.PaymentNavigation != null)
                    .Sum(a => (decimal?)a.StatementNavigation.Price) ?? 0m,

                OffersBoughtCnt = ctx.Announcements
                    .Count(a => a.PaymentNavigation.CustomerId == u.Id),

                TotalSpent = ctx.Announcements
                    .Where(a => a.PaymentNavigation.CustomerId == u.Id)
                    .Sum(a => (decimal?)a.StatementNavigation.Price) ?? 0m
            })
            .ToListAsync();

        return res;
    }
}