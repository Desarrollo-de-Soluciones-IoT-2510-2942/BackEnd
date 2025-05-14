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
    [CustomValidation(typeof(UpdateSubscriptionCommand), nameof(ValidateStartDate))]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(UpdateSubscriptionCommand), nameof(ValidateEndDate))]
    public DateTime EndDate { get; set; }

    public static ValidationResult? ValidateStartDate(DateTime startDate, ValidationContext context)
    {
        if (startDate < DateTime.Today)
        {
            return new ValidationResult("La fecha de inicio no puede ser anterior a la fecha actual.");
        }
        return ValidationResult.Success;
    }

    public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
    {
        var instance = (UpdateSubscriptionCommand)context.ObjectInstance;
        if (endDate <= instance.StartDate)
        {
            return new ValidationResult("La fecha de fin debe ser posterior a la fecha de inicio.");
        }
        return ValidationResult.Success;
    }
}