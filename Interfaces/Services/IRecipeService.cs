using Recipee.DTO;
using Recipee.Entity;
using Ricettario.DTO;

namespace Recipee.Interfaces.Services
{
    public interface IRecipeService
    {
        bool DeleteRecipe(int id);
        List<RecipeShort> GetAllRecipee();
        List<RecipeDTO>? GetRecipeeToSerialize();
        RecipeDTO? GetRecipeId(int id);
        bool InsertRecipe(RecipeDTO recipe);
        bool InsertRecipeeDeserialized(RecipeeDTO listOfRecipeeDeserialized);
        RecipeDTO? UpdateRecipe(int id, RecipeDTO recipe);
    }
}
