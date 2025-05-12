using NutriControl.Domain.IAM.Models;

namespace NutriControl.Domain.IAM.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    
    Task<int?> ValidateToken(string token);
    
}