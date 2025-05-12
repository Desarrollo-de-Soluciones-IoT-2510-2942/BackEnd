using _1_API.Response;
using NutriControl.Domain.IAM.Models;
using NutriControl.Domain.IAM.Queries;

namespace NutriControl.Domain.IAM.Services;

public interface IUserQueryService
{
    Task<User?> GetUserByIdAsync(GetUserByIdQuery query);
    Task<UserResponse?> Handle(GetUserByUserNameQuery query);
    Task<List<UserResponse?>> Handle(GetUserAllQuery query);
    Task<List<UserResponse?>> Handle(GetUserRoleSearchQuery query);
    Task<UserResponse?> Handle(GetUserByDniOrRucQuery query);
}