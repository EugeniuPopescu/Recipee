using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;

namespace Recipee.Interfaces.Services
{
    public interface IImageService
    {
        bool DeleteImage(int id);
        Image GetImageById(int id);
        List<Image> GetImageByRecypeId(int recipeId);
        bool InsertImage(int recipeId, [FromForm] ImageDTO img);
        bool UpdateImage(int id, ImageDTO img);
    }
}
