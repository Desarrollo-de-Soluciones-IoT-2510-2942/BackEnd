

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
}