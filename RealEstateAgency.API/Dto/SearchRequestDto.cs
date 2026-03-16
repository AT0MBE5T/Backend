namespace RealEstateAgency.API.Dto;

public class SearchRequestDto
{
    public string Text { get; set; } = string.Empty;
    public List<string> Filters { get; set; } = [];
    public int SortId { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public Guid? UserId { get; set; }
}