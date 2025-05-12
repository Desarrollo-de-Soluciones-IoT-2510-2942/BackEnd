using Presentation.Request;

namespace Domain;

public interface ISubscriptionCommandService
{
    Task<int> Handle(CreateSubscriptionCommand command);
    Task<bool> Handle(UpdateSubscriptionCommand command);
    Task<bool> Handle(DeleteSubscriptionCommand command); 
}