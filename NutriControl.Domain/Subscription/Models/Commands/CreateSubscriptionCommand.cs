using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain;

namespace Presentation.Request;

public class CreateSubscriptionCommand
{
    [JsonIgnore]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "El tipo de plan es obligatorio.")]
    public PlanType PlanType { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}