using Model.DTOs;
using Repository.GenericRepository;
using Repository.ImageRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.ProductImage
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageRepository _imageRepository;
        public ProductImageService(IUnitOfWork unitOfWork, IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<Data.Entities.ProductImage>> Add(CreateProductImageDto productImageDto)
        {
            var productFromDb = await _unitOfWork.Products.Get(x => x.Id == productImageDto.ProductId);
            var productImageFromDb = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);

            if (productFromDb == null || productImageFromDb.Count == 0)
                return null;

            if (productImageFromDb.Count == 1 && productImageFromDb[0].PublicId == "")
            {
                for (int i = 0; i < productImageDto.Images.Count; i++)
                {
                    var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                    int sortOrder = productImageFromDb[0].SortOrder + i;
                    var productImage = new Data.Entities.ProductImage()
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString(),
                        PublicId = uploadResult.PublicId,
                        Caption = GenerateImageCaption(productFromDb.Name, sortOrder),
                        ProductId = productFromDb.Id,
                        SortOrder = sortOrder
                    };
                    await _unitOfWork.ProductImages.Insert(productImage);
                }
                await _unitOfWork.ProductImages.Delete(productImageFromDb[0].Id);
                await _unitOfWork.Save();
                return await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
            }
            for (int i = 0; i < productImageDto.Images.Count; i++)
            {
                var uploadResult = await _imageRepository.UploadImage("product", productImageDto.Images[i]);
                int sortOrder = productImageFromDb[productImageFromDb.Count - 1].SortOrder + i++;
                var productImage = new Data.Entities.ProductImage()
                {
                    ImageUrl = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId,
                    Caption = GenerateImageCaption(productFromDb.Name, sortOrder),
                    ProductId = productFromDb.Id,
                    SortOrder = sortOrder
                };
                await _unitOfWork.ProductImages.Insert(productImage);
            }
            await _unitOfWork.Save();
            return await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productImageDto.ProductId);
        }

        public async Task<bool> Update(int productId, UpdateProductImageDto productImageDto)
        {
            var productImageFromDb = await _unitOfWork.ProductImages.GetAll(x => x.ProductId == productId);

            if (productImageDto.Images == null || productImageFromDb.Count == 0)
                return false;

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
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var productImage = await _unitOfWork.ProductImages.Get(x => x.Id == id);

            if (productImage == null)
                return false;

            await _imageRepository.DeleteImage(productImage.PublicId);
            await _unitOfWork.ProductImages.Delete(productImage.Id);
            await _unitOfWork.Save();
            return true;
        }
        private static string GenerateImageCaption(string name, int sortOrder)
        {
            return name.ToLower().Replace(" ", "_") + "_image_" + sortOrder;
        }
    }
}
