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
    public class VendorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VendorController> _logger;
        private readonly IMapper _mapper;

        public VendorController(IUnitOfWork unitOfWork, ILogger<VendorController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _unitOfWork.Vendors.GetAll();
            var results = _mapper.Map<List<VendorDto>>(vendors);
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetVendor(int id)
        {
            var vendor = await _unitOfWork.Vendors.Get(x => x.Id == id, null);
            var result = _mapper.Map<VendorDto>(vendor);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDto vendorDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateVendor)}");
                return BadRequest(ModelState);
            }
            var vendor = _mapper.Map<Vendor>(vendorDto);
            await _unitOfWork.Vendors.Insert(vendor);
            await _unitOfWork.Save();
            return CreatedAtAction(nameof(CreateVendor), vendor);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] UpdateVendorDto vendorDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateVendor)}");
                return BadRequest(ModelState);
            }
            var vendor = await _unitOfWork.Vendors.Get(x => x.Id == id);
            if (vendor == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateVendor)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(vendorDto, vendor);
            _unitOfWork.Vendors.Update(vendor);
            await _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteVendor)}");
                return BadRequest();
            }
            var vendor = await _unitOfWork.Vendors.Get(x => x.Id == id);
            if (vendor == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteVendor)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Vendors.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
