using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Core.Dtos;

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
    Task<string> ChangeUserAvatarAsync(ChangeAvatarCommandDto commandDto);
    Task<List<UserGridDto>> GetAll();
    Task<bool> Delete(Guid userId);
    Task<RegistrationResponseDto> RegisterAsync(RegisterCommandDto commandDto);
    Task<RegistrationResponseDto> LoginAsync(LoginCommandDto commandDto);
    Task<List<string>> ChangePasswordAsync(ChangePasswordCommandDto commandDto);
    Task<List<string>> ChangeEmailAsync(ChangeEmailCommandDto commandDto);
    Task<List<string>> ChangePhoneAsync(ChangePhoneCommandDto commandDto);
}