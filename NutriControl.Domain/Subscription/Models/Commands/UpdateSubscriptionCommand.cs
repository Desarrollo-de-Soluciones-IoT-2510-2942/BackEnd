using System.ComponentModel.DataAnnotations;
using Domain;

namespace Presentation.Request;

public class UpdateSubscriptionCommand
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de plan es obligatorio.")]
    public PlanType PlanType { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}