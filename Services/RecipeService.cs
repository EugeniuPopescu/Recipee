using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using Recipee.Interfaces.Services;
using Recipee.Repositories;
using Ricettario.DTO;

namespace Recipee.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public bool InsertRecipeeDeserialized(RecipeeDTO listOfRecipeeDeserialized)
        {
            if (listOfRecipeeDeserialized == null)
            {
                return false;
            }

            try
            {

                foreach (var recipeDTO in listOfRecipeeDeserialized.Recipee)
                {
                    _recipeRepository.InsertRecipee(recipeDTO);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<RecipeShort> GetAllRecipee()
        {
            try
            {
                List<RecipeShort> listOfRecipe = _recipeRepository.GetAllRecipee();

                return listOfRecipe;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RecipeDTO? GetRecipeId(int id)
        {
            try
            {
                RecipeDTO? recipe = _recipeRepository.GetRecipeId(id);

                return recipe;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertRecipe(RecipeDTO recipe)
        {
            if (recipe == null)
            {
                return false;
            }

            try
            {
                List<IngredientDTO>? ingredients = recipe.Ingredients;

                if (ingredients == null)
                {
                    return false;
                }

                bool insertRecipe = _recipeRepository.InsertRecipe(recipe, ingredients);

                if (!insertRecipe)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RecipeDTO? UpdateRecipe(int id, RecipeDTO recipe)
        {
            try
            {
                List<IngredientDTO>? ingredients = recipe.Ingredients;

                RecipeDTO? updateRecipe = _recipeRepository.UpdateRecipe(id, recipe, ingredients);

                return updateRecipe;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteRecipe(int id)
        {
            try
            {
                bool deleteRecipe = _recipeRepository.DeleteRecipe(id);

                if (!deleteRecipe)
                {
                    return false;
                }

                return deleteRecipe;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<RecipeDTO>? GetRecipeeToSerialize()
        {
            try
            {
                List<RecipeDTO>? listRecipe = _recipeRepository.GetRecipeeToSerialize();

                return listRecipe;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
