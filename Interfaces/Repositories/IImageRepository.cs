using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;

namespace Recipee.Interfaces.Repositories
{
    public interface IImageRepository
    {
        bool DeleteImage(int id);
        List<ImageShort>? GetAllImages();
        EntityImage? GetImageById(int id);
        List<EntityImage>? GetImageByRecipeId(int recipeId);
        bool InsertImage(int recipeId, EntityImage image);
        bool UpdateImage(int id, EntityImage image);
    }
}
