
using AutoMapper;
using Domain;
using NutriControl.Domain.Fields.Models.Entities;
using NutriControl.Domain.IAM.Models.Comands;
using Presentation.Request;


namespace _1_API.Mapper;

public class RequestToModels : Profile
{
    public RequestToModels()
    {

        CreateMap<SingupCommand, User>();
        CreateMap<SigninCommand, User>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<CreateSubscriptionCommand, Subscription>();
        CreateMap<UpdateSubscriptionCommand, Subscription>();
        CreateMap<CreateFieldCommand, Field>();
        CreateMap<UpdateFieldCommand, Field>();
        CreateMap<CreateCropCommand, Crop>();
        CreateMap<UpdateCropCommand, Crop>();
        CreateMap<CreateRecommendationCommand, Recommendation>();
        CreateMap<UpdateRecommendationCommand, Recommendation>();
        CreateMap<CreateHistoryCommand, History>();
        CreateMap<UpdateHistoryCommand, History>();
        CreateMap<CreateDeviceCommand, Device>();
        CreateMap<UpdateDeviceCommand, Device>();
        CreateMap<CreateSensorCommand, Sensor>();
        CreateMap<UpdateSensorCommand, Sensor>();
        CreateMap<CreateSensorReadingCommand, SensorReading>();
        CreateMap<UpdateSensorReadingCommand, SensorReading>();
        CreateMap<CreateAlertCommand, Alert>();
        CreateMap<UpdateAlertCommand, Alert>();
    }
}