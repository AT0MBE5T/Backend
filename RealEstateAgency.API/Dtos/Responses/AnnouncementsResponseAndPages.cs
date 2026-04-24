namespace RealEstateAgency.API.Dtos.Responses;

public class AnnouncementsResponseAndPages
{
    public List<AnnouncementResponse> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}