using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dtos.Requests;

public class ChangeEmailRequest
{
    [RegularExpression(@"^(\w|[.-])+@(\w|-)+\.(\w|-){2,4}$",  ErrorMessage = "Invalid email address")]
    public required string NewEmail { get; set; }
}