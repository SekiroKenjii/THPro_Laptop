using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Security;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(ISecurityService securityService, ILogger<AuthenticationController> logger)
        {
            _securityService = securityService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Authenticate)}");
                return BadRequest(ModelState);
            }
            var result = await _securityService.Authenticate(loginDto);
            if(result == null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Authenticate)}");
                return BadRequest("Invalid login information");
            }
            return Ok(result);
        }
    }
}
