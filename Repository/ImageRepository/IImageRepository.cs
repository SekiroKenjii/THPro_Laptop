using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Repository.ImageRepository
{
    public interface IImageRepository
    {
        Task<ImageUploadResult> UploadImage(string entity, IFormFile file);
        Task<DeletionResult> DeleteImage(string publicID);
    }
}
