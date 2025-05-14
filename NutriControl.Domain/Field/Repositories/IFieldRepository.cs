using NutriControl.Domain.Fields.Models.Entities;

namespace Domain;

public interface IFieldRepository
{
    Task<List<Field>> GetAllFieldsAsync();
   
    Task<Field> GetFieldByIdAsync(int id);
    
    Task<List<Field>> GetFieldsByUserIdAsync(int userId);
    
    Task<Field> GetFieldByNameAsync(string name);
    
    Task<int>  SaveFieldAsync(Field dataField);
    Task<bool> UpdateFieldAsync(Field dataField, int id);
    Task<bool> DeleteFieldAsync(int id);
}