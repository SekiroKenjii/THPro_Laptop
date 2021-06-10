using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Role;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet("api/roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetRoles();
            return Ok(result);
        }

        [HttpPost("api/role/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddRole)}");
                return BadRequest(ModelState);
            }
            var result = await _roleService.AddRole(roleDto);
            if (result == null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddRole)}");
                return BadRequest("Submitted data is invalid");
            }
            return CreatedAtAction(nameof(AddRole), result);
        }

        [HttpPut("api/role/update/{roleId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRole)}");
                return BadRequest(ModelState);
            }
            var result = await _roleService.UpdateRole(roleId, roleDto);
            if (result == false)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRole)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpDelete("api/role/delete/{roleId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRole)}");
                return BadRequest(ModelState);
            }
            var result = await _roleService.DeleteRole(roleId);
            if (result == false)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRole)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
