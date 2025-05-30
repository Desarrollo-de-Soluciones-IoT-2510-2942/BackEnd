using _1_API.Response;
using AutoMapper;
using Domain;
using NutriControl.Domain.Crop.Models.Queries;

namespace Application;

public class CropQueryService: ICropQueryService
{
    private readonly ICropRepository _cropRepository;
    private readonly IMapper _mapper;

    public CropQueryService(ICropRepository cropRepository, IMapper mapper)
    {
        _cropRepository = cropRepository;
        _mapper = mapper;
    }

    public async Task<List<CropResponse>?> Handle(GetAllCropsQuery query)
    {
        var data = await _cropRepository.GetAllCropsAsync();
        var result = _mapper.Map<List<Crop>, List<CropResponse>>(data);
        return result;
    }

    public async Task<CropResponse?> Handle(GetCropByIdQuery query)
    {
        var data = await _cropRepository.GetCropByIdAsync(query.Id);
        var result = _mapper.Map<Crop, CropResponse>(data);
        return result;
    }

    public async Task<List<CropResponse>?> Handle(GetCropsByFieldId query)
    {
        var data = await _cropRepository.GetCropsByFieldIdAsync(query.FieldId);
        var result = _mapper.Map<List<Crop>, List<CropResponse>>(data);
        return result;
    }
    
    
    public async Task<Recommendation?> Handle(GetRecommendationByIdQuery query)
    {
        var data = await _cropRepository.GetRecommendationByIdAsync(query.Id);
        return data;
    }
    
    public async Task<List<RecommendationResponse>?> Handle(GetAllRecomendationsForCropQuery query)
    {
        var data = await _cropRepository.GetRecommendationsByCropIdAsync(query.cropId);
        var result = _mapper.Map<List<Recommendation>, List<RecommendationResponse>>(data);
        return result;
    }
    
    public async Task<History?> Handle(GetHistoryByIdQuery query)
    {
        var data = await _cropRepository.GetHistoryByIdAsync(query.Id);
        return data;
    }
    
    public async Task<List<HistoryResponse>?> Handle(GetAllHistoriesForCropQueryQuery forCropQueryQuery)
    {
        var data = await _cropRepository.GetHistoriesByCropIdAsync(forCropQueryQuery.cropId);
        var result = _mapper.Map<List<History>, List<HistoryResponse>>(data);
        return result;
    }
    
    
    
    
}