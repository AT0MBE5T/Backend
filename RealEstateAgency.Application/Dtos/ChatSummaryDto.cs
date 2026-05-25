namespace RealEstateAgency.Application.Dtos;

public record ChatSummaryDto(
    Guid ChatId,
    string ChatName,
    string? LastMessage,
    DateTime? LastMessageAt,
    int UnreadCount,
    string? AvatarUrl,
    DateTime? ClosedAt,
    Guid? OfferId,
    string? OfferTitle,
    Guid? RealtorId,
    string? LastMessageBy,
    Guid? SupportId,
    Guid ChatTypeId
);