using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;
        public SecurityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }

        public async Task<object> Authenticate(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.AccountName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginDto.AccountName);
            }
            await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);
            
            var roles = await _userManager.GetRolesAsync(user);
            var fullName = user.FirstName + " " + user.LastName;

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, fullName),
                new Claim(ClaimTypes.Role, string.Join(";",roles))
            };

            var accessToken = _token.GenerateAccessToken(claims);
            var refreshToken = _token.GenerateRefreshToken();

            user.AccessToken = accessToken;

            if(loginDto.RememberMe == true)
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(15);
            else
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                user = user,
                accessToken = accessToken,
                refreshToken = refreshToken
            });
        }
    }
}
