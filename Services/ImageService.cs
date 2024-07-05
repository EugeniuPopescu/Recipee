using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using Recipee.Interfaces.Services;
using ImageMagick;

namespace Recipee.Services
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        private IConfiguration _configuration;
        private string? _downloadDirectory;
        private int _resizedWidth;
        private int _resizedHeight;

        public ImageService(IImageRepository imageRepository, IConfiguration configuration)
        {
            _imageRepository = imageRepository;
            _configuration = configuration;
            _downloadDirectory = _configuration.GetValue<string>("DownloadDirectory");
            _resizedWidth = _configuration.GetValue<int>("ResizedWidth");
            _resizedHeight = _configuration.GetValue<int>("ResizedHeight");
        }

        public List<ImageShort> GetAllImages()
        {
            List<ImageShort> images = _imageRepository.GetAllImages();

            if (images == null)
            {
                return null;
            }

            return images;
        }

        public EntityImage GetImageById(int id)
        {
            EntityImage image = _imageRepository.GetImageById(id);

            if (image == null)
            {
                return null;
            }
            return image;
        }

        public List<EntityImage> GetImageByRecypeId(int recipeId)
        {
            List<EntityImage> images = _imageRepository.GetImageByRecipeId(recipeId);

            if (images == null)
            {
                return null;
            }

            return images;
        }

        public bool InsertImage(int recipeId, ImageDTO img)
        {
            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {
                // copio l'immagine originale nello stream di memoria
                img.Image.CopyTo(memoryStream);

                // magick core 
                using (MagickImage resizeImage = new MagickImage(memoryStream.ToArray()))
                {
                    MagickGeometry size = new MagickGeometry(_resizedWidth, _resizedHeight);

                    size.FillArea = true;

                    resizeImage.Resize(size);

                    bytes = memoryStream.ToArray();
                }
                

                // creo un oggetto ImageE con i dati dell'immagine ridimensionata
                EntityImage image = new EntityImage()
                {
                    RecipeId = recipeId,
                    FileName = img.Image.FileName,
                    ContentType = img.Image.ContentType,
                    Data = bytes
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
                EntityImage image = new EntityImage()
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
