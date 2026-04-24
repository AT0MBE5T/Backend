namespace RealEstateAgency.API.Dtos.Requests;

public class BanRequest
{
    public Guid UserId { get; set; }
    public DateTime? BanTime { get; set; }
}