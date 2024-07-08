using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;

namespace Recipee.Interfaces.Services
{
    public interface IImageService
    {
        bool DeleteImage(int id);
        byte[]? DownloadImage(List<EntityImage> images);
        List<ImageShort>? GetAllImages();
        EntityImage? GetImageById(int id);
        List<EntityImage>? GetImageByRecypeId(int recipeId);
        bool InsertImage(int recipeId, [FromForm] ImageDTO img);
        bool UpdateImage(int id, ImageDTO img);
    }
}
