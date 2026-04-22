using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dto;

public class ChangePhoneRequestDto
{
    [RegularExpression(@"^\+\d{12}$",  ErrorMessage = "Invalid phone number")]
    public required string NewPhone { get; set; }
}