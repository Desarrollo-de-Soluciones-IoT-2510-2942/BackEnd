using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;

public class CreateSensorReadingCommand
{
    [Required(ErrorMessage = "El ID del sensor es obligatorio.")]
    public int SensorId { get; set; }

    [Required(ErrorMessage = "La fecha y hora de la lectura es obligatoria.")]
    public DateTime Timestamp { get; set; }

    [Required(ErrorMessage = "El valor de la lectura es obligatorio.")]
    public double Value { get; set; }
}