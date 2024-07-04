using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using Recipee.Interfaces.Services;
using Recipee.Repositories;
using System.Net;

namespace Recipee.Services
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        private IConfiguration _configuration;
        private string? _downloadDirectory;

        public ImageService(IImageRepository imageRepository, IConfiguration configuration)
        {
            _imageRepository = imageRepository;
            _configuration = configuration;
            _downloadDirectory = _configuration.GetValue<string>("DownloadDirectory");
        }

        public Image GetImageById(int id)
        {
            Image image = _imageRepository.GetImageById(id);

            if (image == null)
            {
                return null;
            }
            return image;
        }

        public List<Image> GetImageByRecypeId(int recipeId)
        {
            List<Image> images = _imageRepository.GetImageByRecipeId(recipeId);

            if (images == null)
            {
                return null;
            }

            return images;
        }

        public bool InsertImage(int recipeId, ImageDTO img)
        {
            using (var memoryStream = new MemoryStream())
            {
                img.Image.CopyTo(memoryStream);
                Image image = new Image()
                {
                    RecipeId = recipeId,
                    FileName = img.Image.FileName,
                    ContentType = img.Image.ContentType,
                    Data = memoryStream.ToArray()
                };

                bool insertImage = _imageRepository.InsertImage(recipeId, image);

                if (!insertImage)
                {
                    return false;
                }

                return insertImage;
            }
        }

        public bool UpdateImage(int id, ImageDTO img)
        {
            using (var memoryStream = new MemoryStream())
            {
                img.Image.CopyTo(memoryStream);
                Image image = new Image()
                {
                    FileName = img.Image.FileName,
                    ContentType = img.Image.ContentType,
                    Data = memoryStream.ToArray()
                };

                bool updateImage = _imageRepository.UpdateImage(id, image);

                return updateImage;
            }
        }

        public bool DeleteImage(int id)
        {
            try
            {
                bool deleteImage = _imageRepository.DeleteImage(id);

                if (!deleteImage)
                {
                    return false;
                }

                return deleteImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
