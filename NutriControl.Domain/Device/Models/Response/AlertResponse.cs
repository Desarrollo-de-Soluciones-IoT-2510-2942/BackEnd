using System.Text.Json.Serialization;
using Domain;

namespace _1_API.Response;

public class AlertResponse
{
    public string Message { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AlertLevel Level { get; set; }
    
    public DateTime Timestamp { get; set; }
}