using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;


public class CreateHistoryCommand
{
    [JsonIgnore]
    public int CropId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "El tipo de ahorro es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El tipo de ahorro debe tener entre 3 y 50 caracteres.")]
    public string SavingsType { get; set; }

    [Required(ErrorMessage = "La cantidad ahorrada es obligatoria.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad ahorrada debe ser mayor a 0.")]
    public decimal AmountSaved { get; set; }

    [Required(ErrorMessage = "La unidad de medida es obligatoria.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "La unidad de medida debe tener entre 1 y 20 caracteres.")]
    public string UnitOfMeasurement { get; set; }

    [Required(ErrorMessage = "El porcentaje ahorrado es obligatorio.")]
    [Range(0, 100, ErrorMessage = "El porcentaje ahorrado debe estar entre 0 y 100.")]
    public double PercentageSaved { get; set; }
}
