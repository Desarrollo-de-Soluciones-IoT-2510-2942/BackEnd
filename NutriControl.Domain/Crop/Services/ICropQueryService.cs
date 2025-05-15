using _1_API.Response;
using NutriControl.Domain.Crop.Models.Queries;

namespace Domain;

public interface ICropQueryService
{
    Task<List<CropResponse>?> Handle(GetAllCropsQuery query);
    Task<CropResponse?> Handle(GetCropByIdQuery query);
    Task<List<CropResponse>?> Handle(GetCropsByFieldId query);

    Task<Recommendation?> Handle(GetRecommendationByIdQuery query);
    
    Task<List<RecommendationResponse>?> Handle(GetAllRecomendationsForCropQuery query);
}