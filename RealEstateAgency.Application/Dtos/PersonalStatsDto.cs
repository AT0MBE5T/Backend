namespace RealEstateAgency.Application.Dtos;

public class PersonalStatsDto
{
    public int PlacedCnt { get; set; }
    public int SoldCnt { get; set; }
    public int BoughtCnt { get; set; }
    public int DaysFromRegistration { get; set; }
    public int PaymentsCnt { get; set; }
    public int QuestionsCnt { get; set; }
    public int AnswersCnt { get; set; }
    public int CommentsCnt { get; set; }
    public decimal TotalMoneyEarned { get; set; }
    public decimal TotalMoneySpent { get; set; }
    public int Views { get; set; }
    public string FavoriteCategory { get; set; } = string.Empty;
}