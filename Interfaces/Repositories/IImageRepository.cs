using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;

namespace Recipee.Interfaces.Repositories
{
    public interface IImageRepository
    {
        bool DeleteImage(int id);
        Image GetImageById(int id);
        List<Image> GetImageByRecipeId(int recipeId);
        bool InsertImage(int recipeId, Image image);
        bool UpdateImage(int id, Image image);
    }
}
