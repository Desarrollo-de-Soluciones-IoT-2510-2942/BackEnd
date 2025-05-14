using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;

public class CreateCropCommand
{
    [JsonIgnore]
    public int FieldId { get; set; }
    
    [Required(ErrorMessage = "El nombre del campo es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del campo debe tener entre 2 y 100 caracteres.")]
    public string FieldName { get; set; }

    [Required(ErrorMessage = "El tipo de cultivo es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de cultivo debe tener entre 3 y 50 caracteres.")]
    public string CropType { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El estado debe tener entre 3 y 20 caracteres.")]
    public string Status { get; set; }
}