using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Token;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;
        public TokenController(UserManager<AppUser> userManager, ITokenService tokenService, ILogger<TokenController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(TokenDto tokenDto)
        {
            if(tokenDto is null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Refresh)}");
                return BadRequest("Invalid client token");
            }
            string accessToken = tokenDto.AccessToken;
            string refreshToken = tokenDto.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.FindFirst(ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.AccessToken != accessToken ||
                user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Refresh)}");
                return BadRequest("Invalid client token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.AccessToken = newAccessToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
