namespace RealEstateAgency.API.Dto;

public class AnnouncementsResponseAndPages
{
    public List<AnnouncementResponse> Data { get; set; } = [];
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}