

using Domain;
using Microsoft.EntityFrameworkCore;
using NutriControl.Contexts;

namespace NutriControl.Infraestructure.Crop.Persistence;

public class CropRepository : ICropRepository
{
     private readonly NutriControlContext _context;

    public CropRepository(NutriControlContext context)
    {
        _context = context;
    }

    public async Task<List<global::Domain.Crop>> GetAllCropsAsync()
    {
        return await _context.Crops.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<global::Domain.Crop> GetCropByIdAsync(int id)
    {
        return await _context.Crops.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Recommendation> GetRecommendationByIdAsync(int id)
    {
        return await _context.Recommendations.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }
    
    public async Task<List<global::Domain.Crop>> GetCropsByFieldIdAsync(int fieldId)
    {
        return await _context.Crops.Where(s => s.FieldId == fieldId && s.IsActive).ToListAsync();
    }

    public async Task<List<Recommendation>> GetRecommendationsByCropIdAsync(int cropId)
    {
        return await _context.Recommendations.Where(s => s.CropId == cropId && s.IsActive).ToListAsync();
    }

    public async Task<int> SaveCropAsync(global::Domain.Crop dataCrop)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                dataCrop.IsActive = true;
                _context.Crops.Add(dataCrop);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return dataCrop.Id;
    }

    public async Task<int> SaveRecommendationAsync(Recommendation dataRecommendation)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                dataRecommendation.IsActive = true;
                _context.Recommendations.Add(dataRecommendation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return dataRecommendation.Id;
    }
    
    public async Task<bool> UpdateCropAsync(global::Domain.Crop dataCrop, int id)
    {
        var existing = await _context.Crops.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;
        
        existing.CropType = dataCrop.CropType;
        existing.Quantity = dataCrop.Quantity;
        existing.Status = dataCrop.Status;
        existing.IsActive = dataCrop.IsActive;

        _context.Crops.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateRecommendationAsync(Recommendation dataRecommendation, int id)
    {
        var existing = await _context.Recommendations.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;
        
        existing.Content = dataRecommendation.Content;
        existing.Type = dataRecommendation.Type;
        existing.Priority = dataRecommendation.Priority;
        existing.IsActive = dataRecommendation.IsActive;

        _context.Recommendations.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCropAsync(int id)
    {
        var existing = await _context.Crops.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Crops.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteRecommendationAsync(int id)
    {
        var existing = await _context.Recommendations.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Recommendations.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    
    public async Task<bool> FieldExistsAsync(int fieldId)
    {
        return await _context.Fields.AnyAsync(f => f.Id == fieldId);
    }
    
}