namespace RealEstateAgency.Application.Dto;

public record ChatSummaryDto(
    Guid ChatId,
    string ChatName,
    string? LastMessage,
    DateTime? LastMessageAt,
    int UnreadCount,
    string? AvatarUrl
);