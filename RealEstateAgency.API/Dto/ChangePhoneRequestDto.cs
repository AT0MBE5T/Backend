using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dto;

public class ChangePhoneRequestDto
{
    public required Guid UserId { get; set; }
    
    [RegularExpression(@"^\+\d{12}$",  ErrorMessage = "Invalid phone number")]
    public required string NewPhone { get; set; }
}