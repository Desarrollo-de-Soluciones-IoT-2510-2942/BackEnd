using NutriControl.Domain.IAM.Models.Comands;
using Presentation.Request;

namespace NutriControl.Domain.IAM.Services;

public interface IUserCommandService
{
    Task<string> Handle(SigninCommand command);
    Task<int> Handle(SingupCommand command);
    
    Task<bool> Handle(UpdateUserCommand command);
    
    Task<bool> Handle(DeleteUserCommand command); 

}