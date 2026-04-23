using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Interfaces.Services;

public interface IIdentityService
{
    Task<List<string>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task<List<string>> ChangeEmailAsync(string userId, string newEmail);
    Task<List<string>> ChangePhoneAsync(string userId, string newPhone);
    Task<bool> UserExistsAsync(string login);
    Task<bool> AddToRoleAsync(string userId, string role);
    Task<List<string>> CreateUserAsync(User user, string password);
}