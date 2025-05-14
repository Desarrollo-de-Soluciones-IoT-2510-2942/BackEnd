using _1_API.Response;
using NutriControl.Domain.Crop.Models.Queries;

namespace Domain;

public interface ICropQueryService
{
    Task<List<CropResponse>?> Handle(GetAllCropsQuery query);
    Task<CropResponse?> Handle(GetCropByIdQuery query);
    Task<List<CropResponse>?> Handle(GetCropsByFieldId query);
}