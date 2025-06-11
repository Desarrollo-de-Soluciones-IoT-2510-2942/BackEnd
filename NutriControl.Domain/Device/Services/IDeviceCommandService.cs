using Presentation.Request;

namespace Domain;

public interface IDeviceCommandService
{
    Task<int> Handle(CreateDeviceCommand command);
    Task<int> Handle(CreateSensorCommand command);
    Task<int> Handle(CreateSensorReadingCommand command);
    Task<int> Handle(CreateAlertCommand command);

    Task<bool> Handle(UpdateDeviceCommand command);
    Task<bool> Handle(UpdateSensorCommand command);
    Task<bool> Handle(UpdateSensorReadingCommand command);
    Task<bool> Handle(UpdateAlertCommand command);
    
    Task<bool> Handle(DeleteDeviceCommand command);
    Task<bool> Handle(DeleteSensorCommand command);
    Task<bool> Handle(DeleteSensorReadingCommand command);
    
    Task<bool> Handle(DeleteAlertCommand command);
    
    
}