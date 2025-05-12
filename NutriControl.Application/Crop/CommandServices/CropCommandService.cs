using System.Data;
using AutoMapper;
using Domain;
using Presentation.Request;
using Shared;

namespace Application;

public class CropCommandService : ICropCommandService
{
    private readonly ICropRepository _cropRepository;
    private readonly IMapper _mapper;

    public CropCommandService(ICropRepository cropRepository, IMapper mapper)
    {
        _cropRepository = cropRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCropCommand command, int userId)
    {
        // Mapear el comando a la entidad Crop
        var crop = _mapper.Map<CreateCropCommand, Crop>(command);

        // Verificar si el cultivo ya existe
        var existingCrop = await _cropRepository.GetCropByIdAsync(crop.Id);
        if (existingCrop != null)
            throw new DuplicateNameException("El cultivo ya existe.");

        // Guardar el cultivo
        return await _cropRepository.SaveCropAsync(crop);
    }

    public async Task<bool> Handle(UpdateCropCommand command)
    {
        var existingCrop = await _cropRepository.GetCropByIdAsync(command.Id);
        var crop = _mapper.Map<UpdateCropCommand, Crop>(command);

        if (existingCrop == null) throw new NotException("Crop not found");

        return await _cropRepository.UpdateCropAsync(crop, crop.Id);
    }

    public async Task<bool> Handle(DeleteCropCommand command)
    {
        var existingCrop = await _cropRepository.GetCropByIdAsync(command.Id);
        if (existingCrop == null) throw new NotException("Crop not found");
        return await _cropRepository.DeleteCropAsync(command.Id);
    }
    
    public async Task<bool> FieldExistsAsync(int fieldId)
    {
        return await _cropRepository.FieldExistsAsync(fieldId);
    }
}