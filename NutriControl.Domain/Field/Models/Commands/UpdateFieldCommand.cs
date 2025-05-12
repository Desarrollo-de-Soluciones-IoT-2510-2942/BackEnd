using System.ComponentModel.DataAnnotations;

namespace Presentation.Request;

public class UpdateFieldCommand
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } 

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    public string Location { get; set; } 

    [Required(ErrorMessage = "El tipo de suelo es obligatorio.")]
    public string SoilType { get; set; } 

    [Required(ErrorMessage = "La elevación es obligatoria.")]
    public double Elevation { get; set; } 
    
}