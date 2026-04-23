namespace RealEstateAgency.Application.Dto;

public record ChangeEmailCommand(Guid UserId, string Email);