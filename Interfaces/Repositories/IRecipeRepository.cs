using Recipee.DTO;
using Recipee.Entity;
using Ricettario.DTO;

namespace Recipee.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        bool DeleteRecipe(int id);
        List<RecipeShort> GetAllRecipee();
        List<RecipeDTO> GetRecipeeToSerialize();
        RecipeDTO GetRecipeId(int id);
        bool InsertRecipe(RecipeDTO recipe, List<IngredientDTO> ingredients);
        bool InsertRecipee(RecipeDTO recipeDTO);
        RecipeDTO UpdateRecipe(int id, RecipeDTO recipe, List<IngredientDTO> ingredients);
    }
}
