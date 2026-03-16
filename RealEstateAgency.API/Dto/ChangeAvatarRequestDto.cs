namespace RealEstateAgency.API.Dto;

public class ChangeAvatarRequestDto
{
    public Guid UserId { get; set; }
    public IFormFile? Avatar { get; set; }
}