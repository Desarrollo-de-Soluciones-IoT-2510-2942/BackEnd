using System.ComponentModel.DataAnnotations;

namespace NutriControl.Domain.IAM.Models.Comands;

public record SigninCommand
{
    [Required(ErrorMessage = "UserName name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 50 characters.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 255 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,25}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }
}