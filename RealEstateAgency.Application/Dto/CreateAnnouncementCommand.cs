namespace RealEstateAgency.Application.Dto;

public record CreateAnnouncementCommand(
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
    List<FileSource> Photos
);