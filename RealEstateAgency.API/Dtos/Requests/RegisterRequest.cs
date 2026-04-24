using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dtos.Requests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Avatar is required")]
    public required IFormFile Avatar { get; set; }
    
    [Required(ErrorMessage = "Login is required")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Incorrect login length (3 - 50)")]
    public required string Login { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(maximumLength: 256, MinimumLength = 5, ErrorMessage = "Incorrect password length (5 - 256)")]
    public required string Password { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^\+\d{12}$", ErrorMessage = "Wrong phone number. Example: +380123456789")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Incorrect name length (3 - 50)")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Incorrect surname length (3 - 50)")]
    public required string Surname { get; set; }

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 130, ErrorMessage = "Incorrect age (1 - 130)")]
    public required int Age { get; set; }
}