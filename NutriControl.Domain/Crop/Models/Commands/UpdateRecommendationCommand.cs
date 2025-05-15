using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;

public class UpdateRecommendationCommand
{
    [JsonIgnore]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El contenido es obligatorio.")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "El contenido debe tener entre 5 y 500 caracteres.")]
    public string Content { get; set; }

    [Required(ErrorMessage = "El tipo es obligatorio.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "El tipo debe tener entre 3 y 30 caracteres.")]
    public string Type { get; set; }
    
    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    [Range(1, 5, ErrorMessage = "La prioridad debe estar entre 1 y 5.")]
    public int Priority { get; set; }
}