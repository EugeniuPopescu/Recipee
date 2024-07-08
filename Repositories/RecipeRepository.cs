using Dapper;
using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using Ricettario.DTO;
using System.Data.SqlClient;

namespace Recipee.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string? _connectionString;

        public RecipeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool InsertRecipee(RecipeDTO? recipeDTO)
        {
            string queryInsertRecipee = @"
                    DECLARE @insertedId TABLE (Id int);

                    INSERT INTO [dbo].[Recipee] ([Name], [Category], [CookingTime], [Difficulty], [Portions])
                    OUTPUT INSERTED.Id Into @insertedId
                    VALUES (@Name, @Category, @CookingTime, @Difficulty, @Portions);

                    SELECT TOP 1 Id FROM @insertedId";

            string queryInsertIngredients = @"INSERT INTO [dbo].[Ingredients] ([RecipeId], [IngredientName], [Quantity], [MeasurementUnit])
                    VALUES (@RecipeId, @IngredientName, @Quantity, @MeasurementUnit)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (recipeDTO == null)
                    {
                        return false;
                    }

                    int recipeId = connection.QuerySingle<int>(queryInsertRecipee, new
                    {
                        Name = recipeDTO.Name,
                        Category = recipeDTO.Category,
                        CookingTime = recipeDTO.CookingTime,
                        Difficulty = recipeDTO.Difficulty,
                        Portions = recipeDTO.Portions
                    });



                    if (recipeDTO.Ingredients == null)
                    {
                        return false;
                    }

                    foreach (IngredientDTO ingredient in recipeDTO.Ingredients)
                    {

                        dynamic rowAffected = connection.Execute(queryInsertIngredients, new
                        {
                            RecipeId = recipeId,
                            IngredientName = ingredient.IngredientName,
                            Quantity = ingredient.Quantity,
                            MeasurementUnit = ingredient.MeasurementUnit
                        });

                        if (rowAffected == null)
                        {
                            return false;
                        }
                    }

                    return true;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<RecipeShort> GetAllRecipee()
        {
            string getAllRecipeeQuery = @"SELECT [Id]
                                                ,[Name]
                                          FROM [Ricettario].[dbo].[Recipee]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    List<RecipeShort> rowsAffected = connection.Query<RecipeShort>(getAllRecipeeQuery).ToList();

                    return rowsAffected;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public RecipeDTO? GetRecipeId(int id)
        {
            string querySelectId = @"SELECT  [Id]
                                  ,[Name]
                                  ,[Category]
                                  ,[CookingTime]
                                  ,[Difficulty]
                                  ,[Portions]
                              FROM [Ricettario].[dbo].[Recipee]
                              WHERE id = @id";

            string querySelectIngredient = @"SELECT [Id]
                                                  ,[RecipeId]
                                                  ,[IngredientName]
                                                  ,[Quantity]
                                                  ,[MeasurementUnit]
                                              FROM [Ricettario].[dbo].[Ingredients]
                                              WHERE RecipeId = @id";


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    RecipeDTO? recipe = connection.QuerySingleOrDefault<RecipeDTO?>(querySelectId, new { id = id });

                    if (recipe == null)
                    {
                        return null;
                    }

                    List<IngredientDTO> ingredients = connection.Query<IngredientDTO>(querySelectIngredient, new { id = id }).ToList();

                    recipe.Ingredients = ingredients;

                    return recipe;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool InsertRecipe(RecipeDTO recipe, List<IngredientDTO> ingredients)
        {
            string queryInsertRecipe = @"DECLARE @insertedId TABLE (Id int);

                    INSERT INTO [dbo].[Recipee] ([Name], [Category], [CookingTime], [Difficulty], [Portions])
                    OUTPUT INSERTED.Id Into @insertedId
                    VALUES (@Name, @Category, @CookingTime, @Difficulty, @Portions);

                    SELECT IDENT_CURRENT('dbo.Recipee')";

            string queryIsertIngredientsRecipe = @"INSERT INTO [dbo].[Ingredients] ([RecipeId], [IngredientName], [Quantity], [MeasurementUnit])
                    VALUES (@RecipeId, @IngredientName, @Quantity, @MeasurementUnit)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    dynamic recipeId = connection.QuerySingle<int>(queryInsertRecipe, new
                    {
                        Name = recipe.Name,
                        Category = recipe.Category,
                        CookingTime = recipe.CookingTime,
                        Difficulty = recipe.Difficulty,
                        Portions = recipe.Portions
                    });

                    if (recipeId == null)
                    {
                        return false;
                    }

                    foreach (var ingredient in ingredients)
                    {
                        dynamic rowAffected = connection.Execute(queryIsertIngredientsRecipe, new
                        {
                            RecipeId = recipeId,
                            IngredientName = ingredient.IngredientName,
                            Quantity = ingredient.Quantity,
                            MeasurementUnit = ingredient.MeasurementUnit
                        });

                        if (rowAffected == null)
                        {
                            return false;
                        }
                    }

                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public RecipeDTO? UpdateRecipe(int id, RecipeDTO recipe, List<IngredientDTO>? ingredients)
        {
            string queryUpdateRecipe = @"UPDATE [dbo].[Recipee] 
                                        SET [Name] = @Name, [Category] = @Category, [CookingTime] = @CookingTime, [Difficulty] = @Difficulty, [Portions] = @Portions
                                        WHERE Id = @id";

            string queryUpdateIngredients = @"UPDATE [dbo].[Ingredients]
                                                SET [IngredientName] = @IngredientName, [Quantity] = @Quantity, [MeasurementUnit] = @MeasurementUnit
                                                WHERE RecipeId = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var recipeId = connection.Execute(queryUpdateRecipe, new
                    {
                        Id = id,
                        Name = recipe.Name,
                        Category = recipe.Category,
                        CookingTime = recipe.CookingTime,
                        Difficulty = recipe.Difficulty,
                        Portions = recipe.Portions
                    });

                    if (ingredients == null)
                    {
                        return null;
                    }

                    foreach (IngredientDTO ingredient in ingredients)
                    {
                        dynamic rowAffected = connection.Execute(queryUpdateIngredients, new
                        {
                            id = id,
                            IngredientName = ingredient.IngredientName,
                            Quantity = ingredient.Quantity,
                            MeasurementUnit = ingredient.MeasurementUnit
                        });
                    }

                    recipe.Ingredients = ingredients;

                    return recipe;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool DeleteRecipe(int id)
        {
            string queryDelete = @"DELETE FROM [Ricettario].[dbo].[Ingredients]
                                    WHERE RecipeId = @id

                                    DELETE FROM [Ricettario].[dbo].[Images]
                                    WHERE RecipeId = @id

                                    DELETE FROM [Ricettario].[dbo].[Recipee]
                                    WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var delete = connection.Execute(queryDelete, new { id = id });

                    if (delete == 0)
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
        }

        public List<RecipeDTO>? GetRecipeeToSerialize()
        {
            string querySelectR = @"SELECT [Id]
                                          ,[Name]
                                          ,[Category]
                                          ,[CookingTime]
                                          ,[Difficulty]
                                          ,[Portions]
                                      FROM [Ricettario].[dbo].[Recipee]";

            string querySelectI = @"SELECT 
                                       [IngredientName]
                                      ,[Quantity]
                                      ,[MeasurementUnit]
                                  FROM [Ricettario].[dbo].[Ingredients]
                                  WHERE RecipeId = @id";



            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<RecipeDTO> listRecipe = connection.Query<RecipeDTO>(querySelectR).ToList();

                if (listRecipe == null)
                {
                    return null;
                }

                foreach (var recipe in listRecipe)
                {
                    List<IngredientDTO> listIngradients = connection.Query<IngredientDTO>(querySelectI, new { id = recipe.Id }).ToList();

                    recipe.Ingredients = listIngradients;
                }

                return listRecipe;
            }
        }
    }
}
