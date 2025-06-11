namespace Domain;

public interface IDeviceRepository
{
    Task<List<Device>> GetAllDevicesAsync();
    Task<List<Sensor>> GetAllSensorsAsync();
    Task<List<SensorReading>> GetAllSensorsReadingAsync();
    Task<List<Alert>> GetAllAlertsAsync();
    
    Task<Device> GetDeviceByIdAsync(int id);
    Task<Sensor> GetSensorByIdAsync(int id);
    Task<SensorReading> GetSensorReadingByIdAsync(int id);
    Task<Alert> GetAlertByIdAsync(int id);
    
    Task<int> SaveDeviceAsync(Device device);
    Task<int> SaveSensorAsync(Sensor sensor);
    Task<int> SaveSensorReadingAsync(SensorReading sensorReading);
    Task<int> SaveAlertAsync(Alert alert);
    
    Task<bool> UpdateDeviceAsync(Device device, int id);
    Task<bool> UpdateSensorAsync(Sensor sensor, int id);
    Task<bool> UpdateSensorReadingAsync(SensorReading sensorReading, int id);
    Task<bool> UpdateAlertAsync(Alert alert, int id);
    
    Task<bool> DeleteDeviceAsync(int id);
    Task<bool> DeleteSensorAsync(int id);
    Task<bool> DeleteSensorReadingAsync(int id);
    Task<bool> DeleteAlertAsync(int id);
    
    Task<Device> GetDeviceByCropIdAsync(int cropId);
    Task<List<Sensor>> GetSensorsByDeviceIdAsync(int deviceId);
    Task<List<SensorReading>> GetSensorsReadingBySensorIdAsync(int sensorId);
    
    Task<List<Alert>> GetAlertsByDeviceIdAsync(int deviceId);
    
}