using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.GenericRepository;
using Repository.ImageRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.Extensions;

namespace API.Controllers
{
    [ApiController]
    public class TrademarkController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TrademarkController> _logger;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        public TrademarkController(IUnitOfWork unitOfWork, ILogger<TrademarkController> logger,
            IMapper mapper, IImageRepository imageRepository)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        [HttpGet("api/trademarks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTrademarks()
        {
            var trademarks = await _unitOfWork.Trademarks.GetAll();
            var results = _mapper.Map<List<TrademarkDto>>(trademarks);
            return Ok(results);
        }

        [HttpGet("api/trademark/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTrademark(int id)
        {
            var trademark = await _unitOfWork.Trademarks.Get(x => x.Id == id, null);
            var result = _mapper.Map<TrademarkDto>(trademark);
            return Ok(result);
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpPost("api/trademark/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTrademark([FromForm] CreateTrademarkDto trademarkDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateTrademark)}");
                return BadRequest(ModelState);
            }
            var uploadResult = await _imageRepository.UploadImage("trademark", trademarkDto.Image);

            var trademark = _mapper.Map<Trademark>(trademarkDto);

            trademark.ImageUrl = uploadResult.SecureUrl.ToString();
            trademark.PublicId = uploadResult.PublicId;

            await _unitOfWork.Trademarks.Insert(trademark);
            await _unitOfWork.Save();
            return CreatedAtAction(nameof(CreateTrademark), trademark);
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpPut("api/trademark/update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrademark(int id, [FromForm] UpdateTrademarkDto trademarkDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateTrademark)}");
                return BadRequest(ModelState);
            }
            var trademark = await _unitOfWork.Trademarks.Get(x => x.Id == id);
            if (trademark == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateTrademark)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(trademarkDto, trademark);

            if (trademarkDto.Image != null)
            {
                await _imageRepository.DeleteImage(trademark.PublicId);
                var uploadResult = await _imageRepository.UploadImage("trademark", trademarkDto.Image);

                trademark.ImageUrl = uploadResult.SecureUrl.ToString();
                trademark.PublicId = uploadResult.PublicId;
            }
            
            _unitOfWork.Trademarks.Update(trademark);
            await _unitOfWork.Save();
            return NoContent();
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpDelete("api/trademark/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrademark(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteTrademark)}");
                return BadRequest();
            }
            var trademark = await _unitOfWork.Trademarks.Get(x => x.Id == id);
            if (trademark == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteTrademark)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Trademarks.Delete(id);
            await _imageRepository.DeleteImage(trademark.PublicId);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
