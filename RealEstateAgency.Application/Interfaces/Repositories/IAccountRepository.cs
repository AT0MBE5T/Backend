using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<User?> GetUserById(Guid userId);
    Task<int> GetViewsDate(Guid userId, DateTime dateTime);
    Task<int> GetViewsDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetPlacedPropertyCntByUserId(Guid userId);
    Task<int> GetPlacedPropertyCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetPlacedPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetSoldPropertyCntByUserId(Guid userId);
    Task<int> GetSoldPropertyCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetSoldPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetBoughtPropertyCntByUserId(Guid userId);
    Task<int> GetBoughtPropertyCntByUserIdDate(Guid userId,  DateTime dateTime);
    Task<int> GetBoughtPropertyCntByUserIdDateSpan(Guid userId,  DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetDaysFromRegisterByUserId(Guid userId);
    Task<int> GetPaymentsCntByUserId(Guid userId);
    Task<int> GetPaymentsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetPaymentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetQuestionsCntByUserId(Guid userId);
    Task<int> GetQuestionsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetQuestionsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetAnswersCntByUserId(Guid userId);
    Task<int> GetAnswersCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetAnswersCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetCommentsCntByUserId(Guid userId);
    Task<int> GetCommentsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetCommentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<decimal> GetTotalMoneyEarnedByUserId(Guid userId);
    Task<decimal> GetTotalMoneySpentByUserId(Guid userId);
    Task<decimal> GetTotalMoneyEarnedByUserIdDate(Guid userId, DateTime dateTime);
    Task<decimal> GetTotalMoneyEarnedByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<decimal> GetTotalMoneySpentByUserIdDate(Guid userId, DateTime dateTime);
    Task<decimal> GetTotalMoneySpentByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<Guid> GetUserIdByLogin(string login);
    //Task<bool> UpdateAsync(User user);
    Task<string> GetFavoriteCategoryDate(Guid userId, DateTime dateTime);
    Task<string> GetFavoriteCategoryDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<List<UserGrid>> GetAllAsync();
}