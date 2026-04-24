namespace RealEstateAgency.Application.Dtos;

public record ChangeEmailCommandDto(Guid UserId, string Email);