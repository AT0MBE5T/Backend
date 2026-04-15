namespace RealEstateAgency.API.Dto;

public class BanRequest
{
    public Guid UserId { get; set; }
    public DateTime? BanTime { get; set; }
}