using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Model.DTOs;
using Repository.ImageRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        public UserService(UserManager<AppUser> userManager, IMapper mapper, IImageRepository imageRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }
        public async Task<UserDto> AddUser(CreateUserDto userDto)
        {
            var user = _mapper.Map<AppUser>(userDto);

            user.LockoutEnd = DateTime.Now;

            var uploadResult = await _imageRepository.UploadImage("user", userDto.Image, userDto.Gender);

            if(uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            user.ProfilePicture = uploadResult.SecureUrl.ToString();
            user.PublicId = uploadResult.PublicId;

            var createUser = await _userManager.CreateAsync(user, userDto.Password);

            if (!createUser.Succeeded)
            {
                await _imageRepository.DeleteImage(user.PublicId);
                return null;
            }

            if (userDto.Role.ToLower() == "admin")
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return _mapper.Map<UserDto>(user);
            }

            if (userDto.Role.ToLower() == "employee")
            {
                IEnumerable<string> empRoles = new List<string>() { "Employee", userDto.SubRole };
                await _userManager.AddToRolesAsync(user, empRoles);
                return _mapper.Map<UserDto>(user);
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> LockUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            user.LockoutEnd = DateTime.Now.AddYears(1000);
            user.LockoutEnabled = true;

            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UnlockUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            user.LockoutEnd = DateTime.Now;
            user.LockoutEnabled = false;

            await _userManager.UpdateAsync(user);
            return false;
        }

        public async Task<bool> UpdateUser(Guid userId, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            _mapper.Map(userDto, user);

            var currentRole = await _userManager.GetRolesAsync(user);

            if (!string.IsNullOrEmpty(userDto.Password) && !string.IsNullOrEmpty(userDto.NewPassword))
            {
                var passwordChange = await _userManager.ChangePasswordAsync(user, userDto.Password, userDto.NewPassword);
                if (!passwordChange.Succeeded)
                {
                    throw new Exception(passwordChange.Errors.ToString());
                }
            }

            if (!string.IsNullOrEmpty(userDto.SubRole))
            {
                for (var i = 1; i < currentRole.Count; i++)
                {
                    await _userManager.RemoveFromRoleAsync(user, currentRole[i]);
                    await _userManager.AddToRoleAsync(user, userDto.SubRole);
                }
            }

            if (userDto.Image != null)
            {
                var delResult = await _imageRepository.DeleteImage(user.PublicId);

                if (delResult.Error != null)
                    return false;

                var uploadResult = await _imageRepository.UploadImage("user", userDto.Image);

                user.ProfilePicture = uploadResult.SecureUrl.ToString();
                user.PublicId = uploadResult.PublicId;
            }

            await _userManager.UpdateAsync(user);
            return true;
        }
    }
}
