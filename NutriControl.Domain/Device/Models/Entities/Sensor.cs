namespace Domain;

public class Sensor: ModelBase
{
    public int DeviceId { get; set; }
    
    public SensorType Type { get; set; }
    
    public string UnitOfMeasurement { get; set; }
    
    public string Status { get; set; }

    
}

public enum SensorType
{
    Temperature,
    Humidity,
    Light,
    Rain,
    pH,
    Nutrients
}