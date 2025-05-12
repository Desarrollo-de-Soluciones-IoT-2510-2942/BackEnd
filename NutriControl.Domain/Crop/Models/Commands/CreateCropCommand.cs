using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;

public class CreateCropCommand
{
    [JsonIgnore]
    public int FieldId { get; set; }

    [Required(ErrorMessage = "El tipo de cultivo es obligatorio.")]
    public string CropType { get; set; } 

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    public int Quantity { get; set; } 

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string Status { get; set; } 
}