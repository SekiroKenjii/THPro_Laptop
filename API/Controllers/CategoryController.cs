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
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetAll();
            var results = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Categories.Get(x => x.Id == id, null);
            var result = _mapper.Map<CategoryDto>(category);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCategory)}");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.Categories.Insert(category);
            await _unitOfWork.Save();
            return CreatedAtAction(nameof(CreateCategory), category);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCategory)}");
                return BadRequest(ModelState);
            }
            var category = await _unitOfWork.Categories.Get(x => x.Id == id);
            if(category == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCategory)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(categoryDto, category);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCategory)}");
                return BadRequest();
            }
            var category = await _unitOfWork.Categories.Get(x => x.Id == id);
            if (category == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteCategory)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Categories.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
