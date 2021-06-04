using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Model.DTOs;
using Repository.Services.Token;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repository.Services.Security
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
                if (user == null)
                    return null;
            }
            var loginResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);

            if (!loginResult.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("GivenName", user.FirstName + " " + user.LastName),
                new Claim("Role", string.Join(";", roles)),
                new Claim("ProfilePicture", user.ProfilePicture)
            };

            var accessToken = _token.GenerateAccessToken(claims);
            var refreshToken = _token.GenerateRefreshToken();

            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;

            if (!loginDto.IsUsingApp)
            {
                if (loginDto.RememberMe == true)
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(15);
                else
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            }
            else
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(365);
            }

            await _userManager.UpdateAsync(user);

            return new {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
