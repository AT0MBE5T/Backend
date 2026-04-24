namespace RealEstateAgency.API.Dtos.Requests;

public class ChangePasswordRequest
{
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}