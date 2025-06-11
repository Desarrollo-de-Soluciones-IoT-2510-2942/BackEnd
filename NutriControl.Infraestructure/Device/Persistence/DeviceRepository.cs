using Domain;
using Microsoft.EntityFrameworkCore;
using NutriControl.Contexts;

namespace Infraestructure;

public class DeviceRepository: IDeviceRepository
{
    private readonly NutriControlContext _context;

    public DeviceRepository(NutriControlContext context)
    {
        _context = context;
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices.Where(d => d.IsActive).ToListAsync();
    }

    public async Task<List<Sensor>> GetAllSensorsAsync()
    {
        return await _context.Sensors.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<List<SensorReading>> GetAllSensorsReadingAsync()
    {
        return await _context.SensorReadings.ToListAsync();
    }
    
    public async Task<List<Alert>> GetAllAlertsAsync()
    {
        return await _context.Alerts.ToListAsync();
    }

    public async Task<Device> GetDeviceByIdAsync(int id)
    {
        return await _context.Devices.SingleOrDefaultAsync(d => d.Id == id && d.IsActive);
    }

    public async Task<Sensor> GetSensorByIdAsync(int id)
    {
        return await _context.Sensors.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<SensorReading> GetSensorReadingByIdAsync(int id)
    {
        return await _context.SensorReadings.SingleOrDefaultAsync(sr => sr.Id == id);
    }
    
    
    public async Task<Alert> GetAlertByIdAsync(int id)
    {
        return await _context.Alerts.SingleOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task<int> SaveDeviceAsync(Device device)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                device.IsActive = true;
                _context.Devices.Add(device);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return device.Id;
    }

    public async Task<int> SaveSensorAsync(Sensor sensor)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                sensor.IsActive = true;
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return sensor.Id;
    }

    public async Task<int> SaveSensorReadingAsync(SensorReading sensorReading)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.SensorReadings.Add(sensorReading);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return sensorReading.Id;
    }
    
    public async Task<int> SaveAlertAsync(Alert alert)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.Alerts.Add(alert);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return alert.Id;
    }


    public async Task<bool> UpdateDeviceAsync(Device device, int id)
    {
        var existing = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (existing == null) return false;

        existing.Name = device.Name;
        existing.CropId = device.CropId;
        existing.IsActive = device.IsActive;

        _context.Devices.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateSensorAsync(Sensor sensor, int id)
    {
        var existing = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

      
        existing.Type = sensor.Type;
        existing.UnitOfMeasurement = sensor.UnitOfMeasurement;
        existing.Status = sensor.Status;
        existing.IsActive = sensor.IsActive;

        _context.Sensors.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateSensorReadingAsync(SensorReading sensorReading, int id)
    {
        var existing = await _context.SensorReadings.FirstOrDefaultAsync(sr => sr.Id == id);
        if (existing == null) return false;
        
        existing.Timestamp = sensorReading.Timestamp;
        existing.Value = sensorReading.Value;

        _context.SensorReadings.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateAlertAsync(Alert alert, int id)
    {
        var existing = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == id);
        if (existing == null) return false;
        
        existing.Message = alert.Message;
        existing.Level = alert.Level;
        existing.Timestamp = alert.Timestamp;

        _context.Alerts.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteDeviceAsync(int id)
    {
        var existing = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Devices.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSensorAsync(int id)
    {
        var existing = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Sensors.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSensorReadingAsync(int id)
    {
        var existing = await _context.SensorReadings.FirstOrDefaultAsync(sr => sr.Id == id);
        if (existing == null) return false;

        _context.SensorReadings.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteAlertAsync(int id)
    {
        var existing = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == id);
        if (existing == null) return false;

        _context.Alerts.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Device> GetDeviceByCropIdAsync(int cropId)
    {
        return await _context.Devices.SingleOrDefaultAsync(d => d.CropId == cropId && d.IsActive);
    }

    public async Task<List<Sensor>> GetSensorsByDeviceIdAsync(int deviceId)
    {
        return await _context.Sensors.Where(s => s.DeviceId == deviceId && s.IsActive).ToListAsync();
    }

    public async Task<List<SensorReading>> GetSensorsReadingBySensorIdAsync(int sensorId)
    {
        return await _context.SensorReadings.Where(sr => sr.SensorId == sensorId).ToListAsync();
    } 
    
    public async Task<List<Alert>> GetAlertsByDeviceIdAsync(int deviceId)
    {
        return await _context.Alerts.Where(a => a.DeviceId == deviceId).ToListAsync();
    }
    
}