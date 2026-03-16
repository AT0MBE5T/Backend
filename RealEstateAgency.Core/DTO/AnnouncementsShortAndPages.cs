namespace RealEstateAgency.Core.DTO;

public class AnnouncementsShortAndPages
{
    public List<AnnouncementShort> Data { get; set; } = [];
    public int PagesCnt { get; set; }
    public int PageSize { get; set; }
}