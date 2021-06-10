using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Model.DTOs;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDto> AddRole(CreateRoleDto roleDto)
        {
            if (await _roleManager.RoleExistsAsync(roleDto.Name))
                return null;
            var role = new AppRole
            {
                Name = roleDto.Name,
                NormalizedName = roleDto.Name,
                Description = roleDto.Description
            };
            await _roleManager.CreateAsync(role);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<bool> DeleteRole(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded) return false;

            return true;
        }

        public async Task<IList<RoleDto>> GetRoles()
        {
            var roles = await _unitOfWork.Roles.GetAll();
            return _mapper.Map<IList<RoleDto>>(roles);
        }

        public async Task<bool> UpdateRole(Guid roleId, UpdateRoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
                return false;

            if (string.IsNullOrEmpty(roleDto.Name)) return false;

            role.Name = roleDto.Name;
            role.Description = roleDto.Description;
            role.NormalizedName = roleDto.Name;

            var result = await _roleManager.UpdateAsync(role);

            if(!result.Succeeded) return false;

            return true;
        }
    }
}
