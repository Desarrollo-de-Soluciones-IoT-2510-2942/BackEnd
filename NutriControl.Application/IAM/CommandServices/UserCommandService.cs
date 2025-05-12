using System.Data;
using Domain;

using NutriControl.Domain.IAM.Models.Comands;
using NutriControl.Domain.IAM.Repositories;
using NutriControl.Domain.IAM.Services;
using Shared;


namespace Application.IAM.CommandServices;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;
    
    public UserCommandService(IUserRepository userRepository,IEncryptService encryptService,ITokenService tokenService)
    {
        _userRepository = userRepository;
        _encryptService = encryptService;
        _tokenService = tokenService;
    }
    
    public async Task<string> Handle(SigninCommand command)
    {
        var existingUser = await _userRepository.GetUserByUserNameAsync(command.Username);
        if (existingUser == null)
        {
            throw new DataException("User doesn't exist");
        }
        
        
        if (!_encryptService.Verify(command.Password, existingUser.PasswordHashed))
        {
            throw new DataException("Invalid password or username");
        }

       return _tokenService.GenerateToken(existingUser);
    }

    public async Task<int> Handle(SingupCommand command)
    {
        var existingUser = await _userRepository.GetUserByUserNameAsync(command.Username);
        if (existingUser != null) throw new ConstraintException("User already exists");
        
        var existingDniOrRuc = await _userRepository.GetUserByDniOrRucAsync(command.DniOrRuc);
        if (existingDniOrRuc != null) throw new DuplicateNameException("Dni or Ruc  already exists");
        
        var user = new User()
        {
            Username = command.Username,
            DniOrRuc = command.DniOrRuc,
            EmailAddress = command.EmailAddress,
            Phone = command.Phone,
            Role = command.Role,
            PasswordHashed = _encryptService.Encrypt(command.PasswordHashed),
            ConfirmPassword =  _encryptService.Encrypt(command.ConfirmPassword),

        };
        
        var result = await _userRepository.RegisterAsync(user);
        return result;
    }
    
    public  async Task<bool> Handle(DeleteUserCommand command)
    {
        var  existingAccount = await _userRepository.GetUserByIdAsync(command.Id);
            
        if (existingAccount == null) 
            throw new NotException("Account not found");
        
        return  await _userRepository.DeleteUserAsync(command.Id);

    }
    
}