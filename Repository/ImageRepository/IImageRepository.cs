using CloudinaryDotNet.Actions;
using Data.Enums;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Repository.ImageRepository
{
    public interface IImageRepository
    {
        Task<ImageUploadResult> UploadImage(string entity, IFormFile file, Gender? gender = null);
        Task<DeletionResult> DeleteImage(string publicID);
    }
}
