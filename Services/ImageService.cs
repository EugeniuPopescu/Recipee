using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using Recipee.Interfaces.Services;
using ImageMagick;
using System.IO.Compression;

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

        public List<ImageShort>? GetAllImages()
        {
            List<ImageShort>? images = _imageRepository.GetAllImages();

            if (images == null)
            {
                return null;
            }

            return images;
        }

        public EntityImage? GetImageById(int id)
        {
            EntityImage? image = _imageRepository.GetImageById(id);

            if (image == null)
            {
                return null;
            }
            return image;
        }

        public List<EntityImage>? GetImageByRecypeId(int recipeId)
        {
            List<EntityImage>? images = _imageRepository.GetImageByRecipeId(recipeId);

            if (images == null)
            {
                return null;
            }

            return images;
        }

        public bool InsertImage(int recipeId, ImageDTO img)
        {
            byte[] bytes;

            // apro il file di memoria
            using (var memoryStream = new MemoryStream())
            {
                bytes = ResizeImage(img, memoryStream);

                // creo un oggetto EntityImage con i dati dell'immagine ridimensionata
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
            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {

                bytes = ResizeImage(img, memoryStream);

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
            catch (Exception)
            {
                throw;
            }
        }

        // Method used by InsertImage & UpadateImage to resize the image
        private byte[] ResizeImage(ImageDTO img, MemoryStream memoryStream)
        {
            byte[] bytes;
            // copio l'immagine originale nello stream di memoria
            img.Image.CopyTo(memoryStream);

            // faccio partire il flusso da 0
            memoryStream.Seek(0, SeekOrigin.Begin);

            // utilizzo MagickImage perche mi serve per ridimensionare l'immagine 
            using (MagickImage resizeImage = new MagickImage(memoryStream.ToArray()))
            {
                // utilizzo Magic Geometry 
                MagickGeometry size = new MagickGeometry(_resizedWidth, _resizedHeight);

                // riempio l'area
                size.FillArea = true;

                // do le dimensioni all'immagine 
                resizeImage.Resize(size);

                // scrivo nell'areo di memeoria 
                resizeImage.Write(memoryStream);

                using (MemoryStream output = new MemoryStream())
                {
                    // scrivo nell'area di memoria 
                    resizeImage.Write(output);

                    bytes = output.ToArray();
                }
            }

            return bytes;
        }

        public byte[]? DownloadImage(List<EntityImage> images)
        {
            byte[] result;

            // apro il file di memoria 
            using (MemoryStream ms = new MemoryStream())
            {
                // apro zip archive
                using (ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    //ciclo sulle immagini 
                    foreach (EntityImage image in images)
                    {
                        if (image.FileName == null || image.Data == null)
                        {
                            return null;
                        }
                        // crea un nuovo file compresso all'interno dell'archivio zip
                        ZipArchiveEntry fileInArchive = zipArchive.CreateEntry(image.FileName, CompressionLevel.Optimal);

                        // apro il file che ho creato 
                        using (Stream entryStream = fileInArchive.Open())
                        {
                            // per ogni immagine apro il file di memorya con il buffer uguale alla dimansione dell'immagine 
                            using (MemoryStream fileToCompressStream = new MemoryStream(image.Data))
                            {
                                // copio i file nel mio memory stream 
                                fileToCompressStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                // faccio partire il flusso da 0
                ms.Seek(0, SeekOrigin.Begin);

                // salvo il risultato in un byte[] perche mi serve per stamparlo alla fine  
                result = ms.ToArray();
            }

            return result;
        }
    }
}
