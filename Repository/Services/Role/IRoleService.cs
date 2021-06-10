using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Role
{
    public interface IRoleService
    {
        Task<RoleDto> AddRole(CreateRoleDto roleDto);
        Task<IList<RoleDto>> GetRoles();
        Task<bool> UpdateRole(Guid roleId, UpdateRoleDto roleDto);
        Task<bool> DeleteRole(Guid roleId);
    }
}
