using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.API.Dto;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Login is required")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Incorrect login length (3 - 50)")]
    public required string Login { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(maximumLength: 256, MinimumLength = 3, ErrorMessage = "Incorrect password length (3 - 256)")]
    public required string Password { get; set; }
}