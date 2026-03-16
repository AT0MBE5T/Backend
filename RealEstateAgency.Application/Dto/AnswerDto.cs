namespace RealEstateAgency.Application.Dto;

public class AnswerDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public required Guid QuestionId { get; set; }
    public required Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}