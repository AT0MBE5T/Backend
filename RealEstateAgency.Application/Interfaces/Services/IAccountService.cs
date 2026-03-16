using RealEstateAgency.Application.Dto;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IAccountService
{
    Task<string> GetNameSurnameById(Guid userId);
    Task<UserDto?> GetUserDtoById(Guid userId);
    Task<PersonalStatsDto> GetPersonalStatsByUserId(Guid userId);
    Task<PersonalStatsDto?> GetReportByUserLoginDate(ReportUserDto reportUserDto);
    Task<PersonalStatsDto?> GetReportByUserLoginDateSpan(ReportUserDto reportUserDto);
    Task<bool> UpdateUserAvatarAsync(Guid userId, string avatarUrl, string avatarPublicId);
}