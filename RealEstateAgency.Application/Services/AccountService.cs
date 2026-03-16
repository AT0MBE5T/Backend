using Microsoft.AspNetCore.Identity;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
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

        return result;
    }

    public async Task<PersonalStatsDto> GetPersonalStatsByUserId(Guid userId)
    {
        var placedCnt = await repository.GetPlacedPropertyCntByUserId(userId);
        var soldCnt = await repository.GetSoldPropertyCntByUserId(userId);
        var boughtCnt = await repository.GetBoughtPropertyCntByUserId(userId);
        var days = await repository.GetDaysFromRegisterByUserId(userId);
        var paymentsCnt = await repository.GetPaymentsCntByUserId(userId);
        var questionsCnt = await repository.GetQuestionsCntByUserId(userId);
        var answersCnt = await repository.GetAnswersCntByUserId(userId);
        var commentsCnt = await repository.GetCommentsCntByUserId(userId);
        var moneyEarned = await repository.GetTotalMoneyEarnedByUserId(userId);
        var moneySpent = await repository.GetTotalMoneySpentByUserId(userId);

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
            TotalMoneySpent = moneySpent
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
            TotalMoneySpent = moneySpent
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
            TotalMoneySpent =  moneySpent
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
}