using _1_API.Response;
using AutoMapper;
using Domain;
using NutriControl.Domain.Subscriptions.Models.Queries;

namespace Application;

public class SubscriptionQueryService : ISubscriptionQueryService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;
    
    public SubscriptionQueryService(ISubscriptionRepository subscriptionRepository,IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<List<SubscriptionResponse>?> Handle(GetAllSusbcriptionsQuery query)
    {
       var data =  await _subscriptionRepository.GetAllSubscriptionAsync();
        var result = _mapper.Map<List<Subscription>, List<SubscriptionResponse>>(data);
        return result;
    }

    public async Task<SubscriptionResponse?> Handle(GetSubscriptionByIdQuery query)
    {
        var data =  await _subscriptionRepository.GetSubscriptionByIdAsync(query.Id);
        var result = _mapper.Map<Subscription, SubscriptionResponse>(data);
        return result;
    }
    
    public async Task<SubscriptionResponse?> Handle(GetSusbcriptionbyUserIdQuery query)
    {
        var data =  await _subscriptionRepository.GetSubscriptionByUserIdAsync(query.UserId);
        var result = _mapper.Map<Subscription, SubscriptionResponse>(data);
        return result;
    }
    
    
}