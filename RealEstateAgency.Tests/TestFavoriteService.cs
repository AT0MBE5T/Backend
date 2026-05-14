using FluentAssertions;
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

public class TestFavoriteService
{
    [Fact]
    public async Task AddFavoriteTest()
    {
        var favoriteRepository = new Mock<IFavoriteRepository>();
        var favoriteService = new Mock<IFavoriteService>();
        var paymentService = new Mock<IPaymentService>();
        var logger = new Mock<ILogger<FavoriteService>>();

        var mapper = new ApplicationMapper();

        var dto = new FavoriteDto
        {
            UserId = Guid.NewGuid(),
            AnnouncementId = Guid.NewGuid()
        };

        paymentService
            .Setup(x => x.IsExistByAnnouncementIdAsync(dto.AnnouncementId))
            .ReturnsAsync(true);

        favoriteService
            .Setup(x => x.IsInFavoriteAsync(new FavoriteDto{ AnnouncementId = dto.AnnouncementId, UserId = dto.UserId, CreatedAt = DateTime.UtcNow }))
            .ReturnsAsync(false);

        favoriteRepository
            .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
            .ReturnsAsync(true);

        var service = new FavoriteService(
            favoriteRepository.Object,
            paymentService.Object,
            mapper,
            logger.Object);

        var result = await service.AddFavoriteAsync(dto);

        result.Should().Be("Announcement already paid");

        paymentService.Verify(
            x => x.IsExistByAnnouncementIdAsync(dto.AnnouncementId),
            Times.Once);

        favoriteRepository.Verify(
            x => x.AddAsync(It.IsAny<Favorite>()),
            Times.Never);
    }
}