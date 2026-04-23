namespace RealEstateAgency.Application.Dto;

public record AnnouncementInfoCommand(Guid AnnouncementId, Guid? UserId);