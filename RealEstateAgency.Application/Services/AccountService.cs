using Microsoft.AspNetCore.Identity;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class AccountService(
    IAccountRepository repository,
    ApplicationMapper mapper,
    UserManager<User> userManager) : IAccountService
{
    public async Task<string> GetNameSurnameById(Guid userId)
    {
        var user = await repository.GetUserById(userId);
        if (user == null)
        {
            return string.Empty;
        }
        
        return user.Name + ' ' + user.Surname;
    }
    
    public async Task<UserDto?> GetUserDtoById(Guid userId)
    {
        var data = await repository.GetUserById(userId);
        if (data == null)
        {
            return null;
        }
        
        var result = mapper.UserToUserDto(data);

        var roles = await userManager.GetRolesAsync(data);

        result.Roles = roles.ToList();

        return result;
    }
    
    public async Task<PersonalStatsDto?> GetReportByUserId(Guid userId)
    {
        var dateFrom = DateTime.MinValue;
        var dateTo = DateTime.MaxValue.AddDays(-2);
        
        var placedCnt = await repository.GetPlacedPropertyCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var soldCnt = await repository.GetSoldPropertyCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var boughtCnt = await repository.GetBoughtPropertyCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var days = await repository.GetDaysFromRegisterByUserId(userId);
        var paymentsCnt = await repository.GetPaymentsCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var questionsCnt = await repository.GetQuestionsCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var answersCnt = await repository.GetAnswersCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var commentsCnt = await repository.GetCommentsCntByUserIdDateSpan(userId, dateFrom, dateTo);
        var moneyEarned = await repository.GetTotalMoneyEarnedByUserIdDateSpan(userId, dateFrom, dateTo);
        var moneySpent = await repository.GetTotalMoneySpentByUserIdDateSpan(userId, dateFrom, dateTo);
        var views = await repository.GetViewsDateSpan(userId, dateFrom, dateTo);
        var favoriteCategory = await repository.GetFavoriteCategoryDateSpan(userId, dateFrom, dateTo);
        
        return new PersonalStatsDto
        {
            PlacedCnt = placedCnt,
            SoldCnt = soldCnt,
            BoughtCnt = boughtCnt,
            AnswersCnt = answersCnt,
            CommentsCnt = commentsCnt,
            DaysFromRegistration = days,
            PaymentsCnt = paymentsCnt,
            QuestionsCnt = questionsCnt,
            TotalMoneyEarned = moneyEarned,
            TotalMoneySpent =  moneySpent,
            Views = views,
            FavoriteCategory = favoriteCategory
        };
    }
    
    public async Task<PersonalStatsDto?> GetReportByUserLoginDate(ReportUserDto reportUserDto)
    {
        var userId = await repository.GetUserIdByLogin(reportUserDto.Login);

        if (userId == Guid.Empty)
        {
            return null;
        }
        
        var placedCnt = await repository.GetPlacedPropertyCntByUserIdDate(userId, reportUserDto.DateFrom);
        var soldCnt = await repository.GetSoldPropertyCntByUserIdDate(userId, reportUserDto.DateFrom);
        var boughtCnt = await repository.GetBoughtPropertyCntByUserIdDate(userId, reportUserDto.DateFrom);
        var days = await repository.GetDaysFromRegisterByUserId(userId);
        var paymentsCnt = await repository.GetPaymentsCntByUserIdDate(userId, reportUserDto.DateFrom);
        var questionsCnt = await repository.GetQuestionsCntByUserIdDate(userId, reportUserDto.DateFrom);
        var answersCnt = await repository.GetAnswersCntByUserIdDate(userId, reportUserDto.DateFrom);
        var commentsCnt = await repository.GetCommentsCntByUserIdDate(userId, reportUserDto.DateFrom);
        var moneyEarned = await repository.GetTotalMoneyEarnedByUserIdDate(userId, reportUserDto.DateFrom);
        var moneySpent = await repository.GetTotalMoneySpentByUserIdDate(userId, reportUserDto.DateFrom);
        var views = await repository.GetViewsDate(userId, reportUserDto.DateFrom);
        var favoriteCategory = await repository.GetFavoriteCategoryDate(userId, reportUserDto.DateFrom);
        
        return new PersonalStatsDto
        {
            PlacedCnt = placedCnt,
            SoldCnt = soldCnt,
            BoughtCnt = boughtCnt,
            AnswersCnt = answersCnt,
            CommentsCnt = commentsCnt,
            DaysFromRegistration = days,
            PaymentsCnt = paymentsCnt,
            QuestionsCnt = questionsCnt,
            TotalMoneyEarned = moneyEarned,
            TotalMoneySpent = moneySpent,
            Views = views,
            FavoriteCategory = favoriteCategory
        };
    }
    
    public async Task<PersonalStatsDto?> GetReportByUserLoginDateSpan(ReportUserDto reportUserDto)
    {
        var userId = await repository.GetUserIdByLogin(reportUserDto.Login);

        if (userId == Guid.Empty)
        {
            return null;
        }
        
        var placedCnt = await repository.GetPlacedPropertyCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var soldCnt = await repository.GetSoldPropertyCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var boughtCnt = await repository.GetBoughtPropertyCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var days = await repository.GetDaysFromRegisterByUserId(userId);
        var paymentsCnt = await repository.GetPaymentsCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var questionsCnt = await repository.GetQuestionsCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var answersCnt = await repository.GetAnswersCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var commentsCnt = await repository.GetCommentsCntByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var moneyEarned = await repository.GetTotalMoneyEarnedByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var moneySpent = await repository.GetTotalMoneySpentByUserIdDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var views = await repository.GetViewsDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        var favoriteCategory = await repository.GetFavoriteCategoryDateSpan(userId, reportUserDto.DateFrom, reportUserDto.DateTo);
        
        return new PersonalStatsDto
        {
            PlacedCnt = placedCnt,
            SoldCnt = soldCnt,
            BoughtCnt = boughtCnt,
            AnswersCnt = answersCnt,
            CommentsCnt = commentsCnt,
            DaysFromRegistration = days,
            PaymentsCnt = paymentsCnt,
            QuestionsCnt = questionsCnt,
            TotalMoneyEarned = moneyEarned,
            TotalMoneySpent =  moneySpent,
            Views = views,
            FavoriteCategory = favoriteCategory
        };
    }
    
    public async Task<bool> UpdateUserAvatarAsync(Guid userId, string avatarUrl, string avatarPublicId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return false;
        
        user.PublicAvatarId = avatarPublicId;
        user.Avatar =  avatarUrl;

        var res = await repository.UpdateAsync(user);
        return res;
    }
    
    public async Task SetRole(Guid userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return;
        
        var oldRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, oldRoles);
        await userManager.AddToRoleAsync(user, roleName);
    }
    
    public async Task SetBan(Guid userId, DateTime? dateTime)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return;
        
        DateTimeOffset? lockoutContent = dateTime.HasValue 
            ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) 
            : null;
        
        await userManager.SetLockoutEndDateAsync(user, lockoutContent);
    }
    
    public async Task<List<UserGrid>> GetAll()
    {
        var result = await repository.GetAllAsync();
        return result;
    }
    
    public async Task<bool> Delete(Guid userId)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return false;

            await userManager.DeleteAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }
}