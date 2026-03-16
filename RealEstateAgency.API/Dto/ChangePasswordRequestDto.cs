namespace RealEstateAgency.API.Dto;

public class ChangePasswordRequestDto
{
    public required Guid UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}