using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.User
{
    public interface IUserService
    {
        Task<UserDto> GetUser(Guid userId);
        Task<IList<UserVM>> UsersInRole(string role);
        Task<UserDto> AddUser(CreateUserDto userDto);
        Task<bool> UpdateUser(Guid userId, UpdateUserDto userDto);
        Task<bool> LockUser(Guid userId);
        Task<bool> UnlockUser(Guid userId);
        Task<UserDto> Register(RegisterUserDto registerUserDto);
    }
}
