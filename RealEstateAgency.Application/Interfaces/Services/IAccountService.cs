using RealEstateAgency.Application.Dto;
using RealEstateAgency.Core.DTO;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAccountService
{
    Task SetRole(Guid userId, string roleName);
    Task SetBan(Guid userId, DateTime? dateTime);
    Task<string> GetNameSurnameById(Guid userId);
    Task<UserDto?> GetUserDtoById(Guid userId);
    Task<PersonalStatsDto?> GetReportByUserLoginDate(ReportUserDto reportUserDto);
    Task<PersonalStatsDto?> GetReportByUserLoginDateSpan(ReportUserDto reportUserDto);
    Task<PersonalStatsDto?> GetReportByUserId(Guid userId);
    Task<bool> UpdateUserAvatarAsync(Guid userId, string avatarUrl, string avatarPublicId);
    Task<List<UserGrid>> GetAll();
    Task<bool> Delete(Guid userId);
}