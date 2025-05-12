using System.ComponentModel.DataAnnotations;

namespace NutriControl.Domain.IAM.Models.Comands;

public record SingupCommand
{
    [Required(ErrorMessage = "UserName name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 50 characters.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "DNI/RUC is required.")]
    [RegularExpression(@"^\d{8,11}$", ErrorMessage = "DNI/RUC must contain only numbers and be between 8 to 11 digits.")]
    public string DniOrRuc { get; set; }

    [Required]
    [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid user role.")]
    public string  Role { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "Enter a valid phone number.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 25 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,255}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string PasswordHashed { get; set; }
    
    [Compare("PasswordHashed", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}