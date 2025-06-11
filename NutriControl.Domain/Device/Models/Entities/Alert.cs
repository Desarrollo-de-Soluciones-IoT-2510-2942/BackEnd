namespace Domain;

public class Alert: ModelBase
{
    public int DeviceId { get; set; }
    public string Message { get; set; }
    public AlertLevel Level { get; set; }
    public DateTime Timestamp { get; set; }
    
}
public enum AlertLevel
{
    Info,
    Warning,
    Critical
}