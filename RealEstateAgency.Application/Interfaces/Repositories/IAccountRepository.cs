using RealEstateAgency.Core.Dtos;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<User?> GetUserById(Guid userId);
    Task<int> GetViewsDate(Guid userId, DateTime dateTime);
    Task<int> GetViewsDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetPlacedPropertyCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetPlacedPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetSoldPropertyCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetSoldPropertyCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetBoughtPropertyCntByUserIdDate(Guid userId,  DateTime dateTime);
    Task<int> GetBoughtPropertyCntByUserIdDateSpan(Guid userId,  DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetDaysFromRegisterByUserId(Guid userId);
    Task<int> GetPaymentsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetPaymentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetQuestionsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetQuestionsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetAnswersCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetAnswersCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<int> GetCommentsCntByUserIdDate(Guid userId, DateTime dateTime);
    Task<int> GetCommentsCntByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<decimal> GetTotalMoneyEarnedByUserIdDate(Guid userId, DateTime dateTime);
    Task<decimal> GetTotalMoneyEarnedByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<decimal> GetTotalMoneySpentByUserIdDate(Guid userId, DateTime dateTime);
    Task<decimal> GetTotalMoneySpentByUserIdDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<Guid> GetUserIdByLogin(string login);
    Task<string> GetFavoriteCategoryDate(Guid userId, DateTime dateTime);
    Task<string> GetFavoriteCategoryDateSpan(Guid userId, DateTime dateTimeFrom, DateTime dateTimeTo);
    Task<List<UserGridDto>> GetAllAsync();
}