using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dtos.Requests;

public class ChangePhoneRequest
{
    [RegularExpression(@"^\+\d{12}$",  ErrorMessage = "Invalid phone number")]
    public required string NewPhone { get; set; }
}