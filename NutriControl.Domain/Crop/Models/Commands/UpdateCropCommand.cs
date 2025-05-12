using System.ComponentModel.DataAnnotations;

namespace Presentation.Request;

public class UpdateCropCommand
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El tipo de cultivo es obligatorio.")]
    public string CropType { get; set; } 

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    public int Quantity { get; set; } 

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public string Status { get; set; } 
}