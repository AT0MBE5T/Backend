namespace RealEstateAgency.Application.Dtos;

public record CloseAnnouncementCommandDto(Guid CustomerId, Guid AnnouncementId);