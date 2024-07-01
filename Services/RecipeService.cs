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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Recipe> GetAllRecipee()
        {
            try
            {
                List<Recipe> listOfRecipe = _recipeRepository.GetAllRecipee();

                if (listOfRecipe == null)
                {
                    return null;
                }

                return listOfRecipe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RecipeDTO GetRecipeId(int id)
        {
            try
            {
                RecipeDTO recipe = _recipeRepository.GetRecipeId(id);

                if (recipe == null)
                {
                    return null;
                }

                return recipe;
            }
            catch (Exception ex)
            {
                throw ex;
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
                List<IngredientDTO> ingredients = recipe.Ingredients;

                bool insertRecipe = _recipeRepository.InsertRecipe(recipe, ingredients);

                if (!insertRecipe)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RecipeDTO UpdateRecipe(int id, RecipeDTO recipe)
        {
            if (id <= 0 || recipe == null)
            {
                return null;
            }

            try
            {
                List<IngredientDTO> ingredients = recipe.Ingredients;

                RecipeDTO updateRecipe = _recipeRepository.UpdateRecipe(id, recipe, ingredients);

                if (updateRecipe == null)
                {
                    return null;
                }

                return updateRecipe;
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RecipeDTO> GetRecipeeToSerialize()
        {
            try
            {
                List<RecipeDTO> listRecipe = _recipeRepository.GetRecipeeToSerialize();
                
                if (listRecipe == null)
                {
                    return null;
                }

                return listRecipe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
