using Model.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.ProductImage
{
    public interface IProductImageService
    {
        Task<IList<Data.Entities.ProductImage>> Add(CreateProductImageDto productImageDto);
        Task<bool> Update(int productId, UpdateProductImageDto productImageDto);
        Task<bool> Delete(int id);
    }
}
