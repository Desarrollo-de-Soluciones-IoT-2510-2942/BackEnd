using System.Data;
using AutoMapper;
using Domain;
using Presentation.Request;
using Shared;

namespace Application;

public class SubscriptionCommandService : ISubscriptionCommandService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateSubscriptionCommand command)
    {
        var subscription = _mapper.Map<CreateSubscriptionCommand, Subscription>(command);

        // Asegúrate de asignar el UserId correctamente
        if (command.UserId == 0)
        {
            throw new ArgumentException("UserId no puede ser 0");
        }
        subscription.UserId = command.UserId;

        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscription.Id);
        if (existingSubscription != null) throw new DuplicateNameException("Subscription already exists");

        var total = (await _subscriptionRepository.GetAllSubscriptionAsync()).Count;
        return await _subscriptionRepository.SaveSubscriptionAsync(subscription);
    }

    public async Task<bool> Handle(UpdateSubscriptionCommand command)
    {
        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(command.Id);
        var subscription = _mapper.Map<UpdateSubscriptionCommand, Subscription>(command);

        if (existingSubscription == null) throw new NotException("Subscription not found");

        return await _subscriptionRepository.UpdateSubscriptionAsync(subscription, subscription.Id);
    }

    public async Task<bool> Handle(DeleteSubscriptionCommand command)
    {
        var existingSubscription = _subscriptionRepository.GetSubscriptionByIdAsync(command.Id);
        if (existingSubscription == null) throw new NotException("Subscription not found");
        return await _subscriptionRepository.DeleteSubscriptionAsync(command.Id);
    }
}