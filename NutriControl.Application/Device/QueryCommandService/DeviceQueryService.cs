using _1_API.Response;
using AutoMapper;
using Domain;
using NutriControl.Domain.Device.Models.Queries;

namespace Application;

public class DeviceQueryService: IDeviceQueryService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public DeviceQueryService(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<List<DeviceResponse>?> Handle(GetAllDevicesQuery query)
    {
        var data = await _deviceRepository.GetAllDevicesAsync();
        return _mapper.Map<List<Device>, List<DeviceResponse>>(data);
    }

    public async Task<List<SensorResponse>?> Handle(GetAllSensorsQuery query)
    {
        var data = await _deviceRepository.GetAllSensorsAsync();
        return _mapper.Map<List<Sensor>, List<SensorResponse>>(data);
    }

    public async Task<List<SensorReadingResponse>?> Handle(GetAllSensorsReadingQuery query)
    {
        var data = await _deviceRepository.GetAllSensorsReadingAsync();
        return _mapper.Map<List<SensorReading>, List<SensorReadingResponse>>(data);
    }
    
    public async Task<List<AlertResponse>?> Handle(GetAllAlertsQuery query)
    {
        var data = await _deviceRepository.GetAllAlertsAsync();
        return _mapper.Map<List<Alert>, List<AlertResponse>>(data);
    }
    
    public async Task<DeviceResponse?> Handle(GetDeviceByIdQuery query)
    {
        var data = await _deviceRepository.GetDeviceByIdAsync(query.Id);
        return _mapper.Map<Device, DeviceResponse>(data);
    }

    public async Task<SensorResponse?> Handle(GetSensorByIdQuery query)
    {
        var data = await _deviceRepository.GetSensorByIdAsync(query.Id);
        return _mapper.Map<Sensor, SensorResponse>(data);
    }

    public async Task<SensorReadingResponse?> Handle(GetSensorReadingByIdQuery query)
    {
        var data = await _deviceRepository.GetSensorReadingByIdAsync(query.Id);
        return _mapper.Map<SensorReading, SensorReadingResponse>(data);
    }
    
    public async Task<AlertResponse?> Handle(GetAlertByIdQuery query)
    {
        var data = await _deviceRepository.GetAlertByIdAsync(query.Id);
        return _mapper.Map<Alert, AlertResponse>(data);
    }

    public async Task<DeviceResponse?> Handlge(GetDeviceByCropIdQuery query)
    {
        var data = await _deviceRepository.GetDeviceByCropIdAsync(query.CropId);
        return _mapper.Map<Device, DeviceResponse>(data);
    }

    public async Task<List<SensorResponse>?> Handle(GetSensorsByDeviceIdQuery query)
    {
        var data = await _deviceRepository.GetSensorsByDeviceIdAsync(query.DeviceId);
        return _mapper.Map<List<Sensor>, List<SensorResponse>>(data);
    }

    public async Task<List<SensorReadingResponse>?> Handle(GetSensorsReadingsBySensorIdQuery query)
    {
        var data = await _deviceRepository.GetSensorsReadingBySensorIdAsync(query.SensorId);
        return _mapper.Map<List<SensorReading>, List<SensorReadingResponse>>(data);
    }
    
    public async Task<List<AlertResponse>?> Handle(GetAlertsByDeviceIdQuery query)
    {
        var data = await _deviceRepository.GetAlertsByDeviceIdAsync(query.DeviceId);
        return _mapper.Map<List<Alert>, List<AlertResponse>>(data);
    }
}