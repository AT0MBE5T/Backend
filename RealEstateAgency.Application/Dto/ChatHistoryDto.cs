namespace RealEstateAgency.Application.Dto;

public record ChatHistoryDto(
    Guid ChatId,
    List<MessageDto> Messages,
    DateTime? LastMessageTimestamp
);