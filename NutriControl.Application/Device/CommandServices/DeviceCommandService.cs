using System.Data;
using AutoMapper;
using Domain;
using Presentation.Request;
using Shared;

namespace Application;

public class DeviceCommandService: IDeviceCommandService
{
      private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly ICropRepository _cropRepository;

    public DeviceCommandService(IDeviceRepository deviceRepository, ICropRepository cropRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _cropRepository = cropRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDeviceCommand command)
    {
        var crop = await _cropRepository.GetCropByIdAsync(command.CropId);
        if (crop == null)
            throw new NotException("El cultivo no existe.");
        
        var deviceByCrop = await _deviceRepository.GetDeviceByCropIdAsync(command.CropId);
        if (deviceByCrop != null)
            throw new DuplicateNameException("Ya existe un dispositivo para este cultivo.");

        var device = _mapper.Map<CreateDeviceCommand, Device>(command);
        var existing = await _deviceRepository.GetDeviceByIdAsync(device.Id);
        if (existing != null)
            throw new DuplicateNameException("El dispositivo ya existe.");
        return await _deviceRepository.SaveDeviceAsync(device);
    }

    public async Task<int> Handle(CreateSensorCommand command)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(command.DeviceId);
        if (device == null)
            throw new NotException("El dispositivo no existe.");

        var sensor = _mapper.Map<CreateSensorCommand, Sensor>(command);
        var existing = await _deviceRepository.GetSensorByIdAsync(sensor.Id);
        if (existing != null)
            throw new DuplicateNameException("El sensor ya existe.");
        return await _deviceRepository.SaveSensorAsync(sensor);
    }

    public async Task<int> Handle(CreateSensorReadingCommand command)
    {
        var sensor = await _deviceRepository.GetSensorByIdAsync(command.SensorId);
        if (sensor == null)
            throw new NotException("El sensor no existe.");

        var reading = _mapper.Map<CreateSensorReadingCommand, SensorReading>(command);
        var existing = await _deviceRepository.GetSensorReadingByIdAsync(reading.Id);
        if (existing != null)
            throw new DuplicateNameException("La lectura ya existe.");
        return await _deviceRepository.SaveSensorReadingAsync(reading);

    }
    
    public async Task<int> Handle(CreateAlertCommand command)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(command.DeviceId);
        if (device == null)
            throw new NotException("El dispositivo no existe.");

        var alert = _mapper.Map<CreateAlertCommand, Alert>(command);
        var existing = await _deviceRepository.GetAlertByIdAsync(alert.Id);
        if (existing != null)
            throw new DuplicateNameException("La alerta ya existe.");
        return await _deviceRepository.SaveAlertAsync(alert);
    }
    
    public async Task<bool> Handle(UpdateDeviceCommand command)
    {
        var existing = await _deviceRepository.GetDeviceByIdAsync(command.Id);
        var device = _mapper.Map<UpdateDeviceCommand, Device>(command);
        if (existing == null) throw new NotException("Device not found");
        return await _deviceRepository.UpdateDeviceAsync(device, device.Id);
    }

    public async Task<bool> Handle(UpdateSensorCommand command)
    {
        var existing = await _deviceRepository.GetSensorByIdAsync(command.Id);
        var sensor = _mapper.Map<UpdateSensorCommand, Sensor>(command);
        if (existing == null) throw new NotException("Sensor not found");
        return await _deviceRepository.UpdateSensorAsync(sensor, sensor.Id);
    }

    public async Task<bool> Handle(UpdateSensorReadingCommand command)
    {
        var existing = await _deviceRepository.GetSensorReadingByIdAsync(command.Id);
        var reading = _mapper.Map<UpdateSensorReadingCommand, SensorReading>(command);
        if (existing == null) throw new NotException("SensorReading not found");
        return await _deviceRepository.UpdateSensorReadingAsync(reading, reading.Id);
    }
    
    public async Task<bool> Handle(UpdateAlertCommand command)
    {
        var existing = await _deviceRepository.GetAlertByIdAsync(command.Id);
        var alert = _mapper.Map<UpdateAlertCommand, Alert>(command);
        if (existing == null) throw new NotException("Alert not found");
        return await _deviceRepository.UpdateAlertAsync(alert, alert.Id);
    }

    public async Task<bool> Handle(DeleteDeviceCommand command)
    {
        var existing = await _deviceRepository.GetDeviceByIdAsync(command.Id);
        if (existing == null) throw new NotException("Device not found");
        return await _deviceRepository.DeleteDeviceAsync(command.Id);
    }

    public async Task<bool> Handle(DeleteSensorCommand command)
    {
        var existing = await _deviceRepository.GetSensorByIdAsync(command.Id);
        if (existing == null) throw new NotException("Sensor not found");
        return await _deviceRepository.DeleteSensorAsync(command.Id);
    }

    public async Task<bool> Handle(DeleteSensorReadingCommand command)
    {
        var existing = await _deviceRepository.GetSensorReadingByIdAsync(command.Id);
        if (existing == null) throw new NotException("SensorReading not found");
        return await _deviceRepository.DeleteSensorReadingAsync(command.Id);
    }
    
    public async Task<bool> Handle(DeleteAlertCommand command)
    {
        var existing = await _deviceRepository.GetAlertByIdAsync(command.Id);
        if (existing == null) throw new NotException("Alert not found");
        return await _deviceRepository.DeleteAlertAsync(command.Id);
    }
}