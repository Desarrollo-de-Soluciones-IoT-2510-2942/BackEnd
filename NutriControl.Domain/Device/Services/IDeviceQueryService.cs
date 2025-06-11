using _1_API.Response;
using NutriControl.Domain.Device.Models.Queries;

namespace Domain;

public interface IDeviceQueryService
{
    Task<List<DeviceResponse>?> Handle(GetAllDevicesQuery query);
    Task<List<SensorResponse>?> Handle(GetAllSensorsQuery query);
    Task<List<SensorReadingResponse>?> Handle(GetAllSensorsReadingQuery query);
    Task<List<AlertResponse>?> Handle(GetAllAlertsQuery query);
    
    Task<DeviceResponse?> Handle(GetDeviceByIdQuery query);
    Task<SensorResponse?> Handle(GetSensorByIdQuery query);
    Task<SensorReadingResponse?> Handle(GetSensorReadingByIdQuery query);
    
    Task<AlertResponse?> Handle(GetAlertByIdQuery query);
    
    
    Task<DeviceResponse?> Handlge(GetDeviceByCropIdQuery query);
    Task<List<SensorResponse>?> Handle(GetSensorsByDeviceIdQuery query);
    Task<List<SensorReadingResponse>?> Handle(GetSensorsReadingsBySensorIdQuery query);
    
    Task<List<AlertResponse>?> Handle(GetAlertsByDeviceIdQuery query);
    
    
    
}