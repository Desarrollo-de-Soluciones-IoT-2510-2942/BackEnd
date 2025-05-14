using _1_API.Response;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.Fields.Models.Queries;

namespace Domain;

public interface IFieldQueryService
{
    Task<List<FieldResponse>?> Handle(GetAllFieldsQuery query);
    Task<FieldResponse?> Handle(GetFieldByIdQuery query);
    Task<List<FieldResponse>?> Handle(GetFieldsByUserIdQuery query);
    Task<FieldResponse?> Handle(GetFieldByNameQuery query);
    
    Task<FieldResponse?> FindFieldByNameAsync(string fieldName);
    
}