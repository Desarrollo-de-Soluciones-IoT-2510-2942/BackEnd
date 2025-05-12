using _1_API.Response;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.Fields.Models.Queries;

namespace Domain;

public interface IFieldQueryService
{
    Task<List<FieldResponse>?> Handle(GetAllFieldsQuery query);
    Task<FieldResponse?> Handle(GetFieldByIdQuery query);
    Task<FieldResponse?> Handle(GetFieldByUserIdQuery query);
    Task<Field?> GetFieldByUserIdDirectAsync(int userId);
    
}