namespace RealEstateAgency.Application.Dto;

public record DeleteAnnouncementCommand(Guid UserId, Guid AnnouncementId);