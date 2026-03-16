namespace RealEstateAgency.Application.Dto;

public record SendMessageDto(
    Guid ChatId,
    string Content
);