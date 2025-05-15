using Presentation.Request;

namespace Domain;

public interface ICropCommandService
{
    Task<int> Handle(CreateCropCommand command);
    Task<bool> Handle(UpdateCropCommand command);
    Task<bool> Handle(DeleteCropCommand command); 
    
    Task<bool> FieldExistsAsync(int fieldId);
    
    Task<int> Handle(CreateRecommendationCommand command);
    Task<bool> Handle(UpdateRecommendationCommand command);
    Task<bool> Handle(DeleteRecommendationCommand command);
    
}