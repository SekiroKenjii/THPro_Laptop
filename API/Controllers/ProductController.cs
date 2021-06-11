using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.Extensions;

namespace API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("api/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Products.GetAll(null, null,
                new List<string> { "Category", "Condition", "Demand", "Trademark", "Vendor", "ProductImages"});
            var results = _mapper.Map<List<ProductDto>>(products);
            return Ok(results);
        }

        [HttpGet("api/product/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _unitOfWork.Products.Get(x => x.Id == id,
                new List<string> { "Category", "Condition", "Demand", "Trademark", "Vendor", "ProductImages"});
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpPost("api/product/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateProduct)}");
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.Insert(product);
            await _unitOfWork.Save();

            var productImage = new ProductImage()
            {
                ProductId = product.Id,
                Caption = ApplicationStaticExtensions.BlankProductImageCaption(product.Name),
                ImageUrl = ApplicationStaticExtensions.blankProductImageUrl,
                PublicId = ApplicationStaticExtensions.blankProductImagePublicId,
                SortOrder = 1
            };

            await _unitOfWork.ProductImages.Insert(productImage);
            await _unitOfWork.Save();

            var createdProduct = await _unitOfWork.Products.Get(x => x.Id == product.Id,
                new List<string> { "Category", "Condition", "Demand", "Trademark", "Vendor", "ProductImages" });

            return CreatedAtAction(nameof(CreateProduct), _mapper.Map<ProductDto>(createdProduct));
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpPut("api/product/update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProduct)}");
                return BadRequest(ModelState);
            }
            var product = await _unitOfWork.Products.Get(x => x.Id == id);
            if (product == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProduct)}");
                return BadRequest("Submitted data is invalid");
            }
            _mapper.Map(productDto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.Save();
            return NoContent();
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + ApplicationStaticExtensions.WarehouseRole)]
        [HttpDelete("api/product/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteProduct)}");
                return BadRequest();
            }
            var product = await _unitOfWork.Products.Get(x => x.Id == id);
            if (product == null)
            {
                _logger.LogError($"Invalid DLETE attempt in {nameof(DeleteProduct)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Products.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
