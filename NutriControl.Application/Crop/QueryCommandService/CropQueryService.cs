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

    public async Task<CropResponse?> Handle(GetCropByFieldId query)
    {
        var data = await _cropRepository.GetCropByFieldIdAsync(query.FieldId);
        var result = _mapper.Map<Crop, CropResponse>(data);
        return result;
    }
}