namespace Domain;

public interface ISubscriptionRepository
{
    Task<List<Subscription>> GetAllSubscriptionAsync();
   
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    
    Task<Subscription> GetSubscriptionByUserIdAsync(int userId);
    
    Task<int>  SaveSubscriptionAsync(Subscription dataSubscription);
    Task<bool> UpdateSubscriptionAsync(Subscription dataSubscription, int id);
    Task<bool> DeleteSubscriptionAsync(int id);
    
}