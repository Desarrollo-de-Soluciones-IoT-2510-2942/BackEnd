using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain;

namespace Presentation.Request;

public class CreateSensorCommand
{
    [Required(ErrorMessage = "El ID del sensor es obligatorio.")]
    public int DeviceId { get; set; }

    [Required(ErrorMessage = "El tipo de sensor es obligatorio.")]
    public SensorType Type { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "La unidad de medida debe tener entre 1 y 20 caracteres.")]
    public string UnitOfMeasurement { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El estado debe tener entre 3 y 20 caracteres.")]
    public string Status { get; set; }
}