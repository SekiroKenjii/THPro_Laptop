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
    [Route("api/[controller]")]
    [ApiController]
    public class DemandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DemandController> _logger;
        private readonly IMapper _mapper;

        public DemandController(IUnitOfWork unitOfWork, ILogger<DemandController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDemands()
        {
            var demand = await _unitOfWork.Demands.GetAll();
            var results = _mapper.Map<List<DemandDto>>(demand);
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDemand(int id)
        {
            var demand = await _unitOfWork.Demands.Get(x => x.Id == id, null);
            var result = _mapper.Map<DemandDto>(demand);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDemand([FromBody] CreateDemandDto demandDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateDemand)}");
                return BadRequest(ModelState);
            }
            var demand = _mapper.Map<Demand>(demandDto);
            await _unitOfWork.Demands.Insert(demand);
            await _unitOfWork.Save();
            return CreatedAtAction(nameof(CreateDemand), demand);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDemand(int id, [FromBody] UpdateDemandDto demandDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateDemand)}");
                return BadRequest(ModelState);
            }
            var demand = await _unitOfWork.Demands.Get(x => x.Id == id);
            if (demand == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateDemand)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(demandDto, demand);
            _unitOfWork.Demands.Update(demand);
            await _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDemand(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteDemand)}");
                return BadRequest();
            }
            var demand = await _unitOfWork.Demands.Get(x => x.Id == id);
            if (demand == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteDemand)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Demands.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
