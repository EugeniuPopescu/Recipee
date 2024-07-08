using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Services;
using Ricettario.DTO;

namespace Recipee.Controllers
{
    [Route("recipee")]
    [ApiController]
    [AllowAnonymous]

    public class RecipeController
    {
        private readonly IJsonService _JsonService;
        private readonly IRecipeService _recipeService;

        public RecipeController(
            IJsonService JsonService,
            IRecipeService recipeService
        )
        {
            _JsonService = JsonService;
            _recipeService = recipeService;
        }

        /// <summary>
        /// Servizio che deserializza il json e popola le tabelle su sql
        /// </summary>
        [HttpPost("import")]
        public IActionResult DeserializeJsonAndPopulateDB()
        {
            try
            {
                // servizio che mi deserializza il json
                RecipeeDTO? listOfRecipeeDeserialized = _JsonService.GetListOfRecipeeDeserialized();

                if (listOfRecipeeDeserialized == null)
                {
                    return new StatusCodeResult(500);
                }

                // servizio che popola il DB
                bool inserted = _recipeService.InsertRecipeeDeserialized(listOfRecipeeDeserialized);

                if (!inserted)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(inserted);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// retrive all recipee
        /// </summary>
        /// <returns>List of all menu</returns>
        [HttpGet("")]
        public IActionResult GetRecipee()
        {
            try
            {
                List<RecipeShort> listOfRecipe = _recipeService.GetAllRecipee();

                if (listOfRecipe == null)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(listOfRecipe);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// retrive complete recipe with ingredient
        /// </summary>
        /// <param name="id"></param>
        /// <returns>All recipe</returns>
        [HttpGet("{id}")]
        public IActionResult GetRecipeId(int id)
        {
            if (id <= 0)
            {
                return new BadRequestResult();
            }

            try
            {
                RecipeDTO? recipe = _recipeService.GetRecipeId(id);

                if (recipe == null)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(recipe);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert in db a recipe
        /// </summary>
        /// <returns>boolen if the recipe was inserted or not</returns>
        [HttpPost("")]
        public IActionResult InsertRecipe(RecipeDTO recipe)
        {
            if (recipe == null)
            {
                return new BadRequestResult();
            }

            try
            {
                bool insertRecipe = _recipeService.InsertRecipe(recipe);

                if (!insertRecipe)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(insertRecipe);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update recipe and his ingredients by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipe"></param>
        /// <returns>recipe updated</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateRecipe(int id, RecipeDTO recipe)
        {
            if (id <= 0 || recipe == null)
            {
                return new BadRequestResult();
            }

            try
            {
                RecipeDTO? updateRecipe = _recipeService.UpdateRecipe(id, recipe);

                if (updateRecipe == null)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(updateRecipe);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delte recipe by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            if (id <= 0)
            {
                return new BadRequestResult();
            }

            try
            {
                bool deleteRecipe = _recipeService.DeleteRecipe(id);

                if (!deleteRecipe)
                {
                    return new StatusCodeResult(500);
                }

                return new OkObjectResult(deleteRecipe);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Servizio che serializza tabelle sql in json
        /// </summary>
        [HttpPost("export")]
        public IActionResult SerializeTablesIntoJson()
        {
            try
            {
                // servizio che andra a leggere le tabelle
                List<RecipeDTO>? listRecipee = _recipeService.GetRecipeeToSerialize();

                if (listRecipee == null)
                {
                    return new StatusCodeResult(500); 
                }

                // servizio che serializza la lista di tabelle
                bool downloadFile = _JsonService.DownloadJson(listRecipee);

                if (!downloadFile)
                {
                    return new StatusCodeResult(500);
                }
                
                return new OkObjectResult(downloadFile);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
