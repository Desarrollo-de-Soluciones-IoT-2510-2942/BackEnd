using System.Data;
using AutoMapper;
using Domain;
using NutriControl.Domain.Fields.Models.Entities;
using Presentation.Request;
using Shared;

namespace Application;

public class FieldCommandService: IFieldCommandService
{
    private readonly IFieldRepository _fieldRepository;
    private readonly IMapper _mapper;

    public FieldCommandService(IFieldRepository fieldRepository, IMapper mapper)
    {
        _fieldRepository = fieldRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateFieldCommand command)
    {
        var field = _mapper.Map<CreateFieldCommand, Field>(command);

        // Asegúrate de asignar el UserId correctamente
        if (command.UserId == 0)
        {
            throw new ArgumentException("UserId no puede ser 0");
        }
        field.UserId = command.UserId;

        var existingSubscription = await _fieldRepository.GetFieldByIdAsync(field.Id);
        if (existingSubscription != null) throw new DuplicateNameException("Field already exists");

        var total = (await _fieldRepository.GetAllFieldsAsync()).Count;
        return await _fieldRepository.SaveFieldAsync(field);
    }

    public async Task<bool> Handle(UpdateFieldCommand command)
    {
        var existingField = await _fieldRepository.GetFieldByIdAsync(command.Id);
        var field = _mapper.Map<UpdateFieldCommand, Field>(command);

        if (existingField == null) throw new NotException("Field not found");

        return await _fieldRepository.UpdateFieldAsync(field, field.Id);
    }

    public async Task<bool> Handle(DeleteFieldCommand command)
    {
        var existingField = await _fieldRepository.GetFieldByIdAsync(command.Id);
        if (existingField == null) throw new NotException("Field not found");
        return await _fieldRepository.DeleteFieldAsync(command.Id);
    }
}