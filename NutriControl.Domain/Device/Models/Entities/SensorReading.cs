namespace Domain;

public class SensorReading: ModelBase
{
    public int SensorId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    
}