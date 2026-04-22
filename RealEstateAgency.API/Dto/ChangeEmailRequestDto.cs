using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dto;

public class ChangeEmailRequestDto
{
    [RegularExpression(@"^(\w|[.-])+@(\w|-)+\.(\w|-){2,4}$",  ErrorMessage = "Invalid email address")]
    public required string NewEmail { get; set; }
}