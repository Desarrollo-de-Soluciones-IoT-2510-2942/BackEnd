using Domain;
using Microsoft.EntityFrameworkCore;
using NutriControl.Contexts;

namespace Infraestructure;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly NutriControlContext _context;

    public SubscriptionRepository(NutriControlContext context)
    {
        _context = context;
    }

    public async Task<List<Subscription>> GetAllSubscriptionAsync()
    {
        return await _context.Subscriptions.Where(s => s.IsActive).ToListAsync();
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        return await _context.Subscriptions.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Subscription> GetSubscriptionByUserIdAsync(int userId)
    {
        return await _context.Subscriptions.SingleOrDefaultAsync(s => s.UserId == userId && s.IsActive);
    }

    public async Task<int> SaveSubscriptionAsync(Subscription dataSubscription)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                dataSubscription.IsActive = true;
                _context.Subscriptions.Add(dataSubscription);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
        return dataSubscription.Id;
    }

    public async Task<bool> UpdateSubscriptionAsync(Subscription dataSubscription, int id)
    {
        var existing = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.PlanType = dataSubscription.PlanType;
        existing.StartDate = dataSubscription.StartDate;
        existing.EndDate = dataSubscription.EndDate;
        existing.IsActive = dataSubscription.IsActive;

        _context.Subscriptions.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSubscriptionAsync(int id)
    {
        var existing = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        if (existing == null) return false;

        existing.IsActive = false;
        _context.Subscriptions.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}