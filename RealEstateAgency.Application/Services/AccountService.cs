using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mapper;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.DTO;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Application.Services;

public class AccountService(
    IAccountRepository repository,
    ApplicationMapper mapper,
    UserManager<User> userManager,
    IImageService imageService,
    IIdentityService identityService,
    IJwtService jwtService,
    IRefreshService refreshService,
    IAuditService auditService,
    SignInManager<User> signInManager) : IAccountService
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
    
    // public async Task<bool> UpdateUserAvatarAsync(Guid userId, string avatarUrl, string avatarPublicId)
    // {
    //     var user = await userManager.FindByIdAsync(userId.ToString());
    //
    //     if (user is null)
    //         return false;
    //     
    //     user.PublicAvatarId = avatarPublicId;
    //     user.Avatar =  avatarUrl;
    //
    //     var res = await repository.UpdateAsync(user);
    //     return res;
    // }
    
    public async Task<string> ChangeUserAvatarAsync(ChangeAvatarCommand command)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user == null) throw new Exception("User not found");
        
        if (!string.IsNullOrEmpty(user.PublicAvatarId))
        {
            await imageService.DeleteImageAsync(user.PublicAvatarId);
        }

        var uploadResult = await imageService.UploadImageAsync(command.FileStream, command.FileName);
        
        user.Avatar = uploadResult.Url;
        user.PublicAvatarId = uploadResult.PublicId;
        
        await userManager.UpdateAsync(user); 

        return user.Avatar;
    }

    public async Task<List<string>> ChangePasswordAsync(ChangePasswordCommand command)
    {
        var result = await identityService.ChangePasswordAsync(command.UserId.ToString(), command.OldPassword, command.NewPassword);
        return result.Count != 0
            ? result
            : [];
    }
    
    public async Task<List<string>> ChangeEmailAsync(ChangeEmailCommand command)
    {
        var result = await identityService.ChangeEmailAsync(command.UserId.ToString(), command.Email);
        return result.Count != 0
            ? result
            : [];
    }
    
    public async Task<List<string>> ChangePhoneAsync(ChangePhoneCommand command)
    {
        var result = await identityService.ChangePhoneAsync(command.UserId.ToString(), command.Phone);
        return result.Count != 0
            ? result
            : [];
    }
    
    public async Task<RegistrationResponseDto> RegisterAsync(RegisterCommand command)
    {
        if (await identityService.UserExistsAsync(command.Login))
            return new RegistrationResponseDto(Guid.Empty, string.Empty, string.Empty, ["User already exists"], StatusCodes.Status401Unauthorized);
        
        var imageResponse = await imageService.UploadImageAsync(command.AvatarStream, command.AvatarFileName);

        var userId = Guid.NewGuid();
        
        var user = new User {
            Id = userId,
            UserName = command.Login, 
            Avatar = imageResponse.Url,
            PublicAvatarId = imageResponse.PublicId,
            Age = command.Age,
            Name = command.Name,
            Surname =  command.Surname,
            PhoneNumber = command.PhoneNumber,
            CreatedAt = DateTime.UtcNow,
            Email = command.Email
        };

        var createResult = await identityService.CreateUserAsync(user, command.Password);
        if (createResult.Count != 0)
            return new RegistrationResponseDto(Guid.Empty, string.Empty, string.Empty, createResult, StatusCodes.Status400BadRequest);
        
        await identityService.AddToRoleAsync(userId.ToString(), Roles.USER);
        
        var accessToken = await jwtService.GenerateAccessToken(user);
        var refreshToken = await refreshService.GenerateRefreshToken(user.Id);
        
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Register),
            Details = $"User {user.UserName} registered",
            UserId = user.Id
        };
        await auditService.InsertAudit(auditDto);
        return new RegistrationResponseDto(userId, accessToken, refreshToken, [], StatusCodes.Status201Created);
    }
    
    public async Task<RegistrationResponseDto> LoginAsync(LoginCommand command)
    {
        var user = await userManager.FindByNameAsync(command.Login);
        if (user == null) return new RegistrationResponseDto(Guid.Empty, string.Empty, string.Empty, ["User not found"], StatusCodes.Status404NotFound);
        
        var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, false);
        if (!result.Succeeded)
        {
            return new RegistrationResponseDto(Guid.Empty, string.Empty, string.Empty, ["Incorrect password"], StatusCodes.Status401Unauthorized);
        }
        
        var accessToken = await jwtService.GenerateAccessToken(user);
        var refreshToken = await refreshService.GenerateRefreshToken(user.Id);
        
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Login),
            UserId = user.Id,
            Details = $"User {user.UserName} logged in"
        };
        await auditService.InsertAudit(auditDto);
        return new RegistrationResponseDto(user.Id, accessToken, refreshToken, [], StatusCodes.Status200OK);
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