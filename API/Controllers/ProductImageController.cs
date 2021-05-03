using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.GenericRepository;
using Repository.ImageRepository;
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
        public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageDto productImageDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateProductImage)}");
                return BadRequest(ModelState);
            }
            var productFromDb = await _unitOfWork.Products.Get(x => x.Id == productImageDto.ProductId);
            var blankProductImage = await _unitOfWork.ProductImages.Get(x => x.ProductId == productImageDto.ProductId);
            if (productFromDb == null || blankProductImage == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(CreateProductImage)}");
                return BadRequest("Submitted data is invalid");
            }
            for (int i = 0; i < productImageDto.Images.Count; i++)
            {
                var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                var productImage = new ProductImage()
                {
                    ImageUrl = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    Caption = ApplicationStaticExtensions.ProductImageCaption(
                        productFromDb.Name, blankProductImage.SortOrder + i),
                    ProductId = productFromDb.Id,
                    SortOrder = blankProductImage.SortOrder + i
                };
                await _unitOfWork.ProductImages.Insert(productImage);
            }
            await _unitOfWork.ProductImages.Delete(blankProductImage.Id);
            await _unitOfWork.Save();
            var created = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
            return CreatedAtAction(nameof(CreateProductImage), created);
        }
    }
}
