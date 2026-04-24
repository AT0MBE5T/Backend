namespace RealEstateAgency.Application.Dtos;

public record ChangePasswordCommandDto(Guid  UserId, string OldPassword, string NewPassword);