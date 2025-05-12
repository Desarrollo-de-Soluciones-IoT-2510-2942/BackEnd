using Presentation.Request;

namespace Domain;

public interface ICropCommandService
{
    Task<int> Handle(CreateCropCommand command,int userId);
    Task<bool> Handle(UpdateCropCommand command);
    Task<bool> Handle(DeleteCropCommand command); 
    
    Task<bool> FieldExistsAsync(int fieldId);
}