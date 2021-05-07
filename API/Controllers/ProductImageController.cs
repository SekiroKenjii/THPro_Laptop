using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.ProductImage;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly ILogger<ProductImageController> _logger;

        public ProductImageController(IProductImageService productImageService, ILogger<ProductImageController> logger)
        {
            _productImageService = productImageService;
            _logger = logger;
        }

        [HttpPost("api/productimage/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProductImage([FromForm] CreateProductImageDto productImageDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddProductImage)}");
                return BadRequest(ModelState);
            }
            var result = await _productImageService.Add(productImageDto);
            if (result == null)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            return CreatedAtAction(nameof(AddProductImage), result);
        }

        [HttpPut("api/productimage/update/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductImage(int productId, [FromForm] UpdateProductImageDto productImageDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProductImage)}");
                return BadRequest(ModelState);
            }
            var result = await _productImageService.Update(productId, productImageDto);
            if (!result)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpDelete("api/productimage/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var result = await _productImageService.Delete(id);
            if (!result)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
