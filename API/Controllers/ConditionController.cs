using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class ConditionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConditionController> _logger;
        private readonly IMapper _mapper;

        public ConditionController(IUnitOfWork unitOfWork, ILogger<ConditionController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("api/conditions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConditions()
        {
            var conditions = await _unitOfWork.Conditions.GetAll();
            var results = _mapper.Map<List<ConditionDto>>(conditions);
            return Ok(results);
        }

        [HttpGet("api/condition/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCondition(int id)
        {
            var condition = await _unitOfWork.Conditions.Get(x => x.Id == id, null);
            var result = _mapper.Map<CategoryDto>(condition);
            return Ok(result);
        }

        [HttpPost("api/condition/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCondition([FromBody] CreateConditionDto conditionDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCondition)}");
                return BadRequest(ModelState);
            }
            var condition = _mapper.Map<Condition>(conditionDto);
            await _unitOfWork.Conditions.Insert(condition);
            await _unitOfWork.Save();
            return CreatedAtAction(nameof(CreateCondition), condition);
        }

        [HttpPut("api/condition/update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCondition(int id, [FromBody] UpdateConditionDto conditionDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCondition)}");
                return BadRequest(ModelState);
            }
            var condition = await _unitOfWork.Conditions.Get(x => x.Id == id);
            if (condition == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCondition)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(conditionDto, condition);
            _unitOfWork.Conditions.Update(condition);
            await _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("api/condition/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCondition(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCondition)}");
                return BadRequest();
            }
            var condition = await _unitOfWork.Conditions.Get(x => x.Id == id);
            if (condition == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteCondition)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Conditions.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
