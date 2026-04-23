namespace RealEstateAgency.Application.Dto;

public record ChangePasswordCommand(Guid  UserId, string OldPassword, string NewPassword);