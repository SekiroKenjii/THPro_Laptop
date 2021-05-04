using AutoMapper;
using Data.Entities;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ProductImageController> _logger;
        private readonly IMapper _mapper;

        public ProductImageController(IUnitOfWork unitOfWork, IImageRepository imageRepository,
            ILogger<ProductImageController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageRepository = imageRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
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
            var productFromDb = await _unitOfWork.Products.Get(x => x.Id == productImageDto.ProductId);
            var productImageFromDb = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
            IList<ProductImage> results = new List<ProductImage>();
            if (productFromDb == null || productImageFromDb.Count == 0)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            if(productImageFromDb.Count == 1 &&
                productImageFromDb[0].PublicId == ApplicationStaticExtensions.blankProductImagePublicId)
            {
                for (int i = 0; i < productImageDto.Images.Count; i++)
                {
                    var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                    var productImage = new ProductImage()
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString(),
                        PublicId = uploadResult.PublicId,
                        Caption = ApplicationStaticExtensions.ProductImageCaption(
                            productFromDb.Name, productImageFromDb[0].SortOrder + i),
                        ProductId = productFromDb.Id,
                        SortOrder = productImageFromDb[0].SortOrder + i
                    };
                    await _unitOfWork.ProductImages.Insert(productImage);
                }
                await _unitOfWork.ProductImages.Delete(productImageFromDb[0].Id);
                await _unitOfWork.Save();
                results = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
                return CreatedAtAction(nameof(AddProductImage), results);
            }
            for (int i = 0; i < productImageDto.Images.Count; i++)
            {
                var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                var productImage = new ProductImage()
                {
                    ImageUrl = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    Caption = ApplicationStaticExtensions.ProductImageCaption(
                        productFromDb.Name, productImageFromDb[productImageFromDb.Count-1].SortOrder + i + 1),
                    ProductId = productFromDb.Id,
                    SortOrder = productImageFromDb[productImageFromDb.Count - 1].SortOrder + i + 1
                };
                await _unitOfWork.ProductImages.Insert(productImage);
            }
            await _unitOfWork.Save();
            results = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
            return CreatedAtAction(nameof(AddProductImage), results);
        }

        [HttpPut("{productId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductImage(int productId, [FromForm] UpdateProductImageDto productImageDto)
        {
            var productImageFromDb = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productId);
            if (productImageDto.Images == null || productImageFromDb.Count == 0)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            for (int i = 0; i < productImageDto.Images.Count; i++)
            {
                foreach (var item in productImageFromDb)
                {
                    if (productImageDto.Images[i].Name == item.Caption)
                    {
                        await _imageRepository.DeleteImage(item.PublicId);
                        var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                        item.ImageUrl = uploadResult.SecureUrl.ToString();
                        item.PublicId = uploadResult.PublicId;
                        _unitOfWork.ProductImages.Update(item);
                        break;
                    }
                }
            }
            await _unitOfWork.Save();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var productImage = await _unitOfWork.ProductImages.Get(x => x.Id == id);
            if (productImage == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            await _imageRepository.DeleteImage(productImage.PublicId);
            await _unitOfWork.ProductImages.Delete(productImage.Id);
            await _unitOfWork.Save();
            return NoContent();
        }
    }
}
