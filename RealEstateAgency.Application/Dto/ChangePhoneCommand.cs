namespace RealEstateAgency.Application.Dto;

public record ChangePhoneCommand(Guid UserId, string Phone);