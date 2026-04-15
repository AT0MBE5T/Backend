namespace RealEstateAgency.API.Dto;

public class RoleRequest
{
    public Guid UserId { get; set; }
    public required string RoleName { get; set; }
}