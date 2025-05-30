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

    public async Task<int> Handle(CreateCropCommand command)
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
    
    
    public async Task<int> Handle(CreateRecommendationCommand command)
    {
        var recommendation = _mapper.Map<CreateRecommendationCommand, Recommendation>(command);

        // Verificar si la recomendacion ya existe
        var existingRecommendation = await _cropRepository.GetRecommendationByIdAsync(recommendation.Id);
        if (existingRecommendation != null)
            throw new DuplicateNameException("La recoemndacion ya existe.");

        // Guardar el cultivo
        return await _cropRepository.SaveRecommendationAsync(recommendation);
    }
    
    public async Task<bool> Handle(UpdateRecommendationCommand command)
    {
        var existingRecommendation = await _cropRepository.GetRecommendationByIdAsync(command.Id);
        var recommendation = _mapper.Map<UpdateRecommendationCommand, Recommendation>(command);

        if (existingRecommendation == null) throw new NotException("Recommendation not found");

        return await _cropRepository.UpdateRecommendationAsync(recommendation, recommendation.Id);
    }
    
    public async Task<bool> Handle(DeleteRecommendationCommand command)
    {
        var existingRecommendation = await _cropRepository.GetRecommendationByIdAsync(command.Id);
        if (existingRecommendation == null) throw new NotException("Recommendation not found");
        return await _cropRepository.DeleteRecommendationAsync(command.Id);
    }
    
    
    public async Task<int> Handle(CreateHistoryCommand command)
    {
        var history = _mapper.Map<CreateHistoryCommand, History>(command);

        
        var existingHistory = await _cropRepository.GetHistoryByIdAsync(history.Id);
        if (existingHistory != null)
            throw new DuplicateNameException("La historia ya existe.");

        
        return await _cropRepository.SaveHistoryAsync(history);
    }
    
    
    public async Task<bool> Handle(UpdateHistoryCommand command)
    {
        var existingHistory = await _cropRepository.GetHistoryByIdAsync(command.Id);
        var history = _mapper.Map<UpdateHistoryCommand, History>(command);

        if (existingHistory == null) throw new NotException("History not found");

        return await _cropRepository.UpdateHistoryAsync(history, history.Id);
    }
    
    public async Task<bool> Handle(DeleteHistoryCommand command)
    {
        var existingHistory = await _cropRepository.GetHistoryByIdAsync(command.Id);
        if (existingHistory == null) throw new NotException("HISTORY not found");
        return await _cropRepository.DeleteHistoryAsync(command.Id);
    }
    
    
    
    
}