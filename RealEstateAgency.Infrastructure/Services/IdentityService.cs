using Microsoft.AspNetCore.Identity;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;

    public IdentityService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<string>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ["User not found"];

        var identityResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        var errors = identityResult.Errors.Select(e => e.Description).ToList();

        return errors;
    }
    
    public async Task<List<string>> ChangeEmailAsync(string userId, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ["User not found"];
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

        var identityResult = await _userManager.ChangeEmailAsync(user, newEmail, token);
        var errors = identityResult.Errors.Select(e => e.Description).ToList();

        return errors;
    }
    
    public async Task<List<string>> ChangePhoneAsync(string userId, string newPhone)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ["User not found"];
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newPhone);

        var identityResult = await _userManager.ChangePhoneNumberAsync(user, newPhone, token);
        var errors = identityResult.Errors.Select(e => e.Description).ToList();

        return errors;
    }
    
    public async Task<bool> UserExistsAsync(string login)
    {
        var user = await _userManager.FindByNameAsync(login);
        return user is not null;
    }
    
    public async Task<bool> AddToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        await _userManager.AddToRoleAsync(user, role);
        return true;
    }
    
    public async Task<List<string>> CreateUserAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return !result.Succeeded
            ? result.Errors.Select(e => e.Description).ToList()
            : [];
    }
}