using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain;

namespace Presentation.Request;

public class UpdateAlertCommand
{
    [JsonIgnore] 
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El mensaje de la alerta es obligatorio.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El mensaje debe tener entre 3 y 200 caracteres.")]
    public string Message { get; set; }

    [Required(ErrorMessage = "El nivel de la alerta es obligatorio.")]
    public AlertLevel Level { get; set; }

    [Required(ErrorMessage = "La fecha y hora de la alerta es obligatoria.")]
    public DateTime Timestamp { get; set; } 
}