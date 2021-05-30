using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.User;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("api/user/inrole/{role}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UsersInRole(string role)
        {
            var users = await _userService.UsersInRole(role);

            if(users == null)
            {
                _logger.LogError($"Invalid GET attempt in {nameof(UsersInRole)}");
                return BadRequest("Submitted data is invalid");
            }

            return Ok(users);
        }

        [HttpGet("api/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var user = await _userService.GetUser(userId);

            if (user == null)
            {
                _logger.LogError($"Invalid GET attempt in {nameof(GetUser)}");
                return BadRequest("Submitted data is invalid");
            }

            return Ok(user);
        }

        [HttpPost("api/user/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromForm] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddUser)}");
                return BadRequest(ModelState);
            }
            var result = await _userService.AddUser(userDto);
            if (result == null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddUser)}");
                return BadRequest("Submitted data is invalid");
            }
            return CreatedAtAction(nameof(AddUser), result);
        }

        [HttpPut("api/user/update/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromForm] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateUser)}");
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateUser(userId, userDto);
            if (!result)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateUser)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpPost("api/user/lock/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LockUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(LockUser)}");
                return BadRequest(ModelState);
            }
            var result = await _userService.LockUser(userId);
            if (!result)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(LockUser)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpPost("api/user/unlock/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UnlockUser)}");
                return BadRequest(ModelState);
            }
            var result = await _userService.UnlockUser(userId);
            if (!result)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UnlockUser)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
