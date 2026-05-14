using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mappers;
using RealEstateAgency.Application.Services;
using RealEstateAgency.Core.Entities;
using Xunit;

namespace Test;

public class TestAccountService
{
    [Fact]
    public async Task SetRoleTest()
    {
        var userManager = MockUserManager();

        var auditService = new Mock<IAuditService>();

        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            CreatedAt = DateTime.UtcNow,
            Age = 55,
            Name = "Test",
            Surname = "SurName"
        };

        userManager
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        userManager
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        userManager
            .Setup(x => x.RemoveFromRolesAsync(
                user,
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        userManager
            .Setup(x => x.AddToRoleAsync(user, "Admin"))
            .ReturnsAsync(IdentityResult.Success);

        var userRepository = new Mock<IAccountRepository>();
        var imageService = new Mock<IImageService>();
        var jwtService = new Mock<IJwtService>();
        var refreshService = new Mock<IRefreshService>();
        var logger = new Mock<ILogger<AccountService>>();
        var identityService = new Mock<IIdentityService>();
        var mapper = new ApplicationMapper();
        
        var service = new AccountService(userRepository.Object, mapper, userManager.Object, imageService.Object, identityService.Object, jwtService.Object, refreshService.Object, auditService.Object, null, logger.Object);
        
        await service.SetRole(userId, "Admin", adminId);
        
        userManager.Verify(
            x => x.RemoveFromRolesAsync(
                user,
                It.Is<IEnumerable<string>>(r => r.Contains("User"))),
            Times.Once);

        userManager.Verify(
            x => x.AddToRoleAsync(user, "Admin"),
            Times.Once);

        auditService.Verify(
            x => x.InsertAudit(It.Is<AuditDto>(a =>
                a.UserId == adminId &&
                a.Details.Contains(userId.ToString()) &&
                a.Details.Contains("Admin"))),
            Times.Once);
    }

    private Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();

        return new Mock<UserManager<User>>(
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }
}