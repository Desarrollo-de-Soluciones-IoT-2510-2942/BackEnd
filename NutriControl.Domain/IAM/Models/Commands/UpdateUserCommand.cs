using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriControl.Domain.IAM.Models.Comands;

public class UpdateUserCommand
{
    [JsonIgnore] 
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El username es obligatorio.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El username debe tener entre 2 y 50 caracteres.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "El DNI o RUC es obligatorio.")]
    [RegularExpression(@"^\d{8,11}$", ErrorMessage = "El DNI o RUC debe contener solo números y tener entre 8 y 11 dígitos.")]
    public string DniOrRuc { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe proporcionar un email válido.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "El teléfono debe contener solo números y tener entre 9 y 12 dígitos.")]
    public string Phone { get; set; }
}