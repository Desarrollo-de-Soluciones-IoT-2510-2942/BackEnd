using Domain;
using Microsoft.EntityFrameworkCore;
using NutriControl.Contexts;

namespace Infraestructure;

public class CropRepository : ICropRepository
{
     private readonly NutriControlContext _context;

    public CropRepository(NutriControlContext context)
    {
        _context = context;
    }

    public async Task<List<Crop>> GetAllCropsAsync()
    {
        return await _context.Crops.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<Crop> GetCropByIdAsync(int id)
    {
        return await _context.Crops.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<List<Crop>> GetCropsByFieldIdAsync(int fieldId)
    {
        return await _context.Crops.Where(s => s.FieldId == fieldId && s.IsActive).ToListAsync();
    }

    public async Task<int> SaveCropAsync(Crop dataCrop)
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

    public async Task<bool> UpdateCropAsync(Crop dataCrop, int id)
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

    public async Task<bool> DeleteCropAsync(int id)
    {
        var existing = await _context.Crops.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Crops.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> FieldExistsAsync(int fieldId)
    {
        return await _context.Fields.AnyAsync(f => f.Id == fieldId);
    }
    
}