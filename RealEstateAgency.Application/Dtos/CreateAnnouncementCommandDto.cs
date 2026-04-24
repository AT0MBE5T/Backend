namespace RealEstateAgency.Application.Dtos;

public record CreateAnnouncementCommandDto(
    Guid UserId,
    Guid PropertyType,
    Guid StatementType,
    string Location,
    double Area,
    int Floors,
    int Rooms,
    string Title,
    decimal Price,
    string Content,
    string Description,
    List<FileSourceDto> Photos
);