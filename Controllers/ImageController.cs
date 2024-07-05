using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Services;
using System.IO.Compression;

namespace Recipee.Controllers
{
    [Route("Images")]
    [ApiController]
    [AllowAnonymous]

    public class ImageController : ControllerBase
    {
        private IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("")]
        public IActionResult GetAllImages()
        {
            List<ImageShort> images = _imageService.GetAllImages();

            if (images == null)
            {
                return new StatusCodeResult(500);
            }


            return new OkObjectResult(images);
        }

        /// <summary>
        /// get images of recipe by id 
        /// </summary>
        /// <param name="id">Insert image Id</param>
        /// <returns>images of specified recipe</returns>
        [HttpGet("{id}")]
        public IActionResult GetImage(int id)
        {
            if (id <= 0)
            {
                return new BadRequestResult();
            }

            EntityImage image = _imageService.GetImageById(id);

            if (image == null)
            {
                return new StatusCodeResult(500);
            }

            return new OkObjectResult(image);
        }

        /// <summary>
        /// Get images of specified recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <returns>all images of the recipe</returns>
        [HttpGet("recipe/{id}/download/")]
        public IActionResult GetImageByRecipeId(int id)
        {
            if (id <= 0)
            {
                return new BadRequestResult();
            }

            List<EntityImage> images = _imageService.GetImageByRecypeId(id);

            if (images == null)
            {
                return new StatusCodeResult(500);
            }

            string zipFileName = "imageZip.zip";

            byte[] result;

            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var image in images)
                    {
                        var fileInArchive = zipArchive.CreateEntry(image.FileName, CompressionLevel.Optimal);
                        using (var entryStream = fileInArchive.Open())
                        { 
                            using (MemoryStream fileToCompressStream = new MemoryStream(image.Data))
                            {
                                fileToCompressStream.CopyTo(entryStream);
                            }
                        }
                    }
                }

                ms.Seek(0, SeekOrigin.Begin);
                result = ms.ToArray();
            }

            return File(result, "application/zip", zipFileName);
        }

        /// <summary>
        /// insert image
        /// </summary>
        /// <param name="recipeId">specify recipe id where want to insert image</param>
        /// <param name="imagePath">image path to insert</param>
        /// <returns>boolean if image was inserted or not</returns>
        [HttpPost("recipe/{recipeId}")]
        public IActionResult PostFile(int recipeId, [FromForm] ImageDTO img)
        {
            if (recipeId <= 0 || img == null || img.Image == null)
            {
                return new BadRequestResult();
            }

            bool insertImage = _imageService.InsertImage(recipeId, img);

            if (!insertImage)
            {
                return new StatusCodeResult(500);
            }

            return new OkObjectResult(insertImage);
        }

        /// <summary>
        /// edit image
        /// </summary>
        /// <param name="id">id image to edit</param>
        /// <param name="img">image to edit</param>
        /// <returns>boolean if image was updated or not</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateImage(int id, [FromForm] ImageDTO img)
        {
            if (id <= 0 || img == null || img.Image == null)
            {
                return new BadRequestResult();
            }

            bool updateImage = _imageService.UpdateImage(id, img);

            if (!updateImage)
            {
                return new StatusCodeResult(500);
            }

            return new OkObjectResult(updateImage);
        }

        /// <summary>
        /// delete Image
        /// </summary>
        /// <param name="id">id of image to delete</param>
        /// <returns>boolean if image was deleted or not</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteImage(int id)
        {
            if (id <= 0)
            {
                return new BadRequestResult();
            }

            bool deleteImage = _imageService.DeleteImage(id);

            if (!deleteImage)
            {
                return new StatusCodeResult(500);
            }

            return new OkObjectResult(deleteImage);
        }
    }
}
