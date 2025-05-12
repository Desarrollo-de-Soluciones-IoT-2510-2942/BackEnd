using _1_API.Response;
using AutoMapper;
using Domain;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.Fields.Models.Queries;


namespace Application;

public class FieldQueryService : IFieldQueryService
{
    private readonly IFieldRepository _fieldRepository;
    private readonly IMapper _mapper;

    public FieldQueryService(IFieldRepository fieldRepository, IMapper mapper)
    {
        _fieldRepository = fieldRepository;
        _mapper = mapper;
    }

    public async Task<List<FieldResponse>?> Handle(GetAllFieldsQuery query)
    {
        var data = await _fieldRepository.GetAllFieldsAsync();
        var result = _mapper.Map<List<Field>, List<FieldResponse>>(data);
        return result;
    }

    public async Task<FieldResponse?> Handle(GetFieldByIdQuery query)
    {
        var data = await _fieldRepository.GetFieldByIdAsync(query.Id);
        var result = _mapper.Map<Field, FieldResponse>(data);
        return result;
    }

    public async Task<FieldResponse?> Handle(GetFieldByUserIdQuery query)
    {
        var data = await _fieldRepository.GetFieldByUserIdAsync(query.UserId);
        var result = _mapper.Map<Field, FieldResponse>(data);
        return result;
    }
    
    public async Task<Field?> GetFieldByUserIdDirectAsync(int userId)
    {
        return await _fieldRepository.GetFieldByUserIdAsync(userId);
    }
}