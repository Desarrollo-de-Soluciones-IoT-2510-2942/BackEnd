
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
        CreateMap<Field, CreateFieldCommand>();
        CreateMap<Crop, CreateCropCommand>();

    }
}