using System.Data;
using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public UserCommandService(IUserRepository userRepository,IEncryptService encryptService,ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _encryptService = encryptService;
        _tokenService = tokenService;
        _mapper = mapper;
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
    
    
    public async Task<bool> Handle(UpdateUserCommand command)
    {
        // Buscar el usuario existente por ID
        var existingUser = await _userRepository.GetUserByIdAsync(command.Id);
        if (existingUser == null)
        {
            throw new NotException("User not found");
        }

        // Mapear los datos del comando al modelo de usuario
        var updatedUser = _mapper.Map<UpdateUserCommand, User>(command);

        // Actualizar el usuario en el repositorio
        var result = await _userRepository.UpdateUserAsync(updatedUser, command.Id);

        // Retornar el resultado
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