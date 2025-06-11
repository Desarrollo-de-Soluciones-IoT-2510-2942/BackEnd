using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Presentation.Request;

public class UpdateDeviceCommand
{
    [JsonIgnore] 
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre del dispositivo es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del dispositivo debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; }
    
}