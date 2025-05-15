

using Domain;

public interface ICropRepository
{
    Task<List<Crop>> GetAllCropsAsync();
   
    Task<Crop> GetCropByIdAsync(int id);
    
    Task<List<Crop>> GetCropsByFieldIdAsync(int fieldId);
    
    Task<int>  SaveCropAsync(Crop dataCrop);
    Task<bool> UpdateCropAsync(Crop dataCrop, int id);
    Task<bool> DeleteCropAsync(int id);
    
    Task<bool> FieldExistsAsync(int fieldId);
    
    
    Task<int>  SaveRecommendationAsync(Recommendation dataRecommendation);
    Task<bool> UpdateRecommendationAsync(Recommendation dataRecommendation, int id);
    Task<bool> DeleteRecommendationAsync(int id);
    Task<List<Recommendation>> GetRecommendationsByCropIdAsync(int cropId);
    Task<Recommendation> GetRecommendationByIdAsync(int id);
    
}