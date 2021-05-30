using Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Role
{
    public interface IRoleService
    {
        Task<RoleDto> AddRole(CreateRoleDto roleDto);
        Task<IList<RoleDto>> GetRoles();
    }
}
