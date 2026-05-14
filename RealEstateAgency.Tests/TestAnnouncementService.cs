using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateAgency.Application.Interfaces.Repositories;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Mappers;
using RealEstateAgency.Application.Services;
using Xunit;

namespace Test;

public class TestAnnouncementService
{
    [Fact]
    public void CalculateAgencyCommission()
    {
        var mockRepo = new Mock<IAnnouncementRepository>();
        var statementService = new Mock<IStatementService>();
        var auditService = new Mock<IAuditService>();
        var propertyService = new Mock<IPropertyService>();
        var imageService = new Mock<IImageService>();
        var verificationRepo = new Mock<IVerificationRepository>();
        var mapper = new Mock<ApplicationMapper>();
        var unitOfWorkRepo = new Mock<IUnitOfWork>();
        var loggerRepo = new Mock<ILogger<AnnouncementService>>();
        var hub = new Mock<IHubService>();
        var paymentService = new Mock<IPaymentService>();

        var service = new AnnouncementService(
            mockRepo.Object,
            statementService.Object,
            auditService.Object,
            propertyService.Object,
            imageService.Object,
            mapper.Object,
            verificationRepo.Object,
            unitOfWorkRepo.Object,
            hub.Object,
            paymentService.Object,
            loggerRepo.Object);

        var propertyPrice = 123456.99m;
        var agencyPercent = 2m;

        var result = service.CalculateAgencyCommission(propertyPrice, agencyPercent);

        result.Should().Be(2469.14m);
    }
    
    [Fact]
    public void CalculateTotalPurchasePrice()
    {
        var mockRepo = new Mock<IAnnouncementRepository>();
        var statementService = new Mock<IStatementService>();
        var auditService = new Mock<IAuditService>();
        var propertyService = new Mock<IPropertyService>();
        var imageService = new Mock<IImageService>();
        var verificationRepo = new Mock<IVerificationRepository>();
        var mapper = new Mock<ApplicationMapper>();
        var unitOfWorkRepo = new Mock<IUnitOfWork>();
        var loggerRepo = new Mock<ILogger<AnnouncementService>>();
        var hub = new Mock<IHubService>();
        var paymentService = new Mock<IPaymentService>();

        var service = new AnnouncementService(
            mockRepo.Object,
            statementService.Object,
            auditService.Object,
            propertyService.Object,
            imageService.Object,
            mapper.Object,
            verificationRepo.Object,
            unitOfWorkRepo.Object,
            hub.Object,
            paymentService.Object,
            loggerRepo.Object);

        var propertyPrice = 123456.99m;
        var agencyCommission = 2469.14m;
        var taxes = 500m;
        var notaryPrice = 1000m;

        var result = service.CalculateTotalPurchasePrice(propertyPrice, agencyCommission, taxes, notaryPrice);

        result.Should().Be(127426.13m);
    }
}