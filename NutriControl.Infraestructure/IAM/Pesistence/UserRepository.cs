using NutriControl.Contexts;
using NutriControl.Domain.IAM;
using NutriControl.Domain.IAM.Repositories;
using Microsoft.EntityFrameworkCore;

namespace NutriControl.Infraestructure.IAM.Persistence;

public class UserRepository : IUserRepository
{
    private readonly NutriControlContext _nutriControlContext;

    public UserRepository(NutriControlContext nutriControlContext)
    {
        _nutriControlContext = nutriControlContext;
    }
    
    public async Task<int> RegisterAsync(User user)
    {
        _nutriControlContext.Users.Add(user);
        await _nutriControlContext.SaveChangesAsync();
        
        return user.Id;
    }

    public async Task<User?> GetUserByUserNameAsync(string username)
    {
       var user = await _nutriControlContext.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsActive);
       return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _nutriControlContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return user;
    }
    
    public async Task<List<User>> GetUserAllAsync()
    {
        var result = await _nutriControlContext.Users.Where(t => t.IsActive).ToListAsync();
        return result;
    }

    public async Task<List<User>> GetUserRoleSearchAsync(string role)
    {
        var result = await _nutriControlContext.Users.Where(x => x.IsActive && x.Role == role).ToListAsync();
        return result;
    }
    

    public async Task<User> GetUserByDniOrRucAsync(string dniOrRucruc)
    {
        return await _nutriControlContext.Users
            .Where(x => x.DniOrRuc == dniOrRucruc && x.IsActive)
            .FirstOrDefaultAsync();
    }

    
    public async Task<bool> UpdateUserAsync(User dataUser, int id)
    {
        var existing = await _nutriControlContext.Users.FirstOrDefaultAsync(f => f.Id == id);
        if (existing == null) return false;

        existing.Username = dataUser.Username;
        existing.DniOrRuc = dataUser.DniOrRuc;
        existing.EmailAddress = dataUser.EmailAddress;
        existing.Phone = dataUser.Phone;

        _nutriControlContext.Users.Update(existing);
        await _nutriControlContext.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<bool> DeleteUserAsync(int id)
    {
        var exitingAccount = await _nutriControlContext.Users.FirstOrDefaultAsync(t => t.Id == id);
        if (exitingAccount != null)
        {
            exitingAccount.IsActive = false;
            _nutriControlContext.Users.Update(exitingAccount);
            await _nutriControlContext.SaveChangesAsync();
            return true;
        }
        return false;
    }    
}