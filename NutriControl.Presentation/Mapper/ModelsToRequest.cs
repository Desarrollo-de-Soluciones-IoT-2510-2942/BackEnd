
using AutoMapper;
using Domain;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.IAM.Models.Comands;
using Presentation.Request;


namespace _1_API.Mapper;

public class ModelsToRequest : Profile
{
    public ModelsToRequest()
    {
      
        CreateMap<User, SingupCommand>();
        CreateMap<User, SigninCommand>();
        CreateMap<Subscription, CreateSubscriptionCommand>();
        CreateMap<Subscription, UpdateSubscriptionCommand>();
        CreateMap<Field, CreateFieldCommand>();
        CreateMap<Field, UpdateFieldCommand>();
        CreateMap<Crop, CreateCropCommand>();
        CreateMap<Crop, UpdateCropCommand>();
        CreateMap<User, UpdateUserCommand>();
        CreateMap<Recommendation, CreateRecommendationCommand>();
        CreateMap<Recommendation, UpdateRecommendationCommand>();

    }
}