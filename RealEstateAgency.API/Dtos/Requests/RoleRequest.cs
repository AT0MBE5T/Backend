namespace RealEstateAgency.API.Dtos.Requests;

public class RoleRequest
{
    public Guid UserId { get; set; }
    public required string RoleName { get; set; }
}