namespace RealEstateAgency.Application.Dtos;

public record DeleteAnnouncementCommandDto(Guid UserId, Guid AnnouncementId);