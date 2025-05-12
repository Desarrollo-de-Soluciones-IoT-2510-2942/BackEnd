using Domain;
using Microsoft.EntityFrameworkCore;
using NutriControl.Contexts;
using NutriControl.Domain.Fields.Models.Entities;

namespace Infraestructure;

public class FieldRepository : IFieldRepository
{
     private readonly NutriControlContext _context;

    public FieldRepository(NutriControlContext context)
    {
        _context = context;
    }

    public async Task<List<Field>> GetAllFieldsAsync()
    {
        return await _context.Fields.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<Field> GetFieldByIdAsync(int id)
    {
        return await _context.Fields.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Field> GetFieldByUserIdAsync(int userId)
    {
        return await _context.Fields.SingleOrDefaultAsync(s => s.UserId == userId && s.IsActive);
    }

    public async Task<int> SaveFieldAsync(Field dataField)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                dataField.IsActive = true;
                _context.Fields.Add(dataField);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return dataField.Id;
    }

    public async Task<bool> UpdateFieldAsync(Field dataField, int id)
    {
        var existing = await _context.Fields.FirstOrDefaultAsync(f => f.Id == id);
        if (existing == null) return false;

        existing.Name = dataField.Name;
        existing.Location = dataField.Location;
        existing.SoilType = dataField.SoilType;
        existing.Elevation = dataField.Elevation;
        existing.IsActive = dataField.IsActive;

        _context.Fields.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFieldAsync(int id)
    {
        var existing = await _context.Fields.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Fields.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}