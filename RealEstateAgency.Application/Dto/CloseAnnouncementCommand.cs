namespace RealEstateAgency.Application.Dto;

public record CloseAnnouncementCommand(Guid CustomerId, Guid AnnouncementId);