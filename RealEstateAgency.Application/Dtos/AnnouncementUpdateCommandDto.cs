namespace RealEstateAgency.Application.Dtos;

public record AnnouncementUpdateCommandDto
(
    Guid AnnouncementId,
    List<FileSourceDto> NewPhotos,
    List<Guid> DeletedImageIds,
    List<string> ExistingImageOrder,
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
    Guid UserId
);