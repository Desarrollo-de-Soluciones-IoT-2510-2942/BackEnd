using System.ComponentModel.DataAnnotations;

namespace Presentation.Request;

public class UpdateFieldCommand
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La ubicación debe tener entre 5 y 200 caracteres.")]
    public string Location { get; set; }

    [Required(ErrorMessage = "El tipo de suelo es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de suelo debe tener entre 3 y 50 caracteres.")]
    public string SoilType { get; set; }

    [Required(ErrorMessage = "La elevación es obligatoria.")]
    [Range(1, 8848, ErrorMessage = "La elevación debe estar entre 1 y 8848 metros.")]
    public double Elevation { get; set; }
    
}