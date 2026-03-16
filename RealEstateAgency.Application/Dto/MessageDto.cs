namespace RealEstateAgency.Application.Dto;

public record MessageDto(
    Guid Id,
    Guid ChatId,
    Guid SenderId,
    string SenderName,
    string Content,
    DateTime CreatedAt,
    bool IsRead = false
);