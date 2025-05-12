using _1_API.Response;
using NutriControl.Domain.Subscriptions.Models.Queries;


namespace Domain;

public interface ISubscriptionQueryService
{
    Task<List<SubscriptionResponse>?> Handle(GetAllSusbcriptionsQuery query);
    Task<SubscriptionResponse?> Handle(GetSubscriptionByIdQuery query);
    Task<SubscriptionResponse?> Handle(GetSusbcriptionbyUserIdQuery query);
}