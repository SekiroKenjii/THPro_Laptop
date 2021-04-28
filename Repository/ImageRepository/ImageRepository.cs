using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Model.Configurations;
using System.IO;
using System.Threading.Tasks;

namespace Repository.ImageRepository
{
    public class ImageRepository : IImageRepository
    {
        private readonly Cloudinary _cloudinary;
        public ImageRepository(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeleteImage(string publicID)
        {
            var deletionParams = new DeletionParams(publicID);
            return await _cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<ImageUploadResult> UploadImage(string entity, IFormFile file)
        {
            var uploadParams = new ImageUploadParams();

            if(file == null)
            {
                uploadParams.File = new FileDescription(GetBlankImage(entity));
                uploadParams.Folder = $"upload/img/{entity}/";

                return await _cloudinary.UploadAsync(uploadParams);
            }

            using var stream = file.OpenReadStream();

            uploadParams.File = new FileDescription(file.FileName, stream);
            uploadParams.Folder = $"upload/img/{entity}/";

            return await _cloudinary.UploadAsync(uploadParams);
        }
        private static string GetBlankImage(string entity)
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).FullName +
                $"\\Assets\\images\\{entity}\\blank_{entity}_image.jpg";
        }
    }
}
