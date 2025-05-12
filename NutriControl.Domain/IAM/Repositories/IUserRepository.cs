namespace NutriControl.Domain.IAM.Repositories;

public interface IUserRepository
{
    Task<int> RegisterAsync(User user);
    Task<User?> GetUserByUserNameAsync(string username);    
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetUserAllAsync();
    Task<List<User>> GetUserRoleSearchAsync(string role);
    Task<User> GetUserByDniOrRucAsync(string dniOrRuc );
    Task<bool> DeleteUserAsync(int id);
}