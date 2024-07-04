﻿using Dapper;
using Microsoft.AspNetCore.Mvc;
using Recipee.DTO;
using Recipee.Entity;
using Recipee.Interfaces.Repositories;
using System.Data.SqlClient;
using System.Net.Mime;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Recipee.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly string? _connectionString;

        public ImageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Image GetImageById(int id)
        {
            string query = "SELECT * FROM [dbo].[Images] WHERE [Id] = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    Image image = connection.QuerySingleOrDefault<Image>(query, new { Id = id });

                    if (image == null)
                    {
                        return null;
                    }

                    return image;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<Image> GetImageByRecipeId(int recipeId)
        {
            string queryImages = @"SELECT [Id]
                                      ,[RecipeId]
                                      ,[FileName]
                                      ,[ContentType]
                                      ,[Data]
                                  FROM [Ricettario].[dbo].[Images]
                                  WHERE RecipeId =  @recipeId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    List<Image> images = connection.Query<Image>(queryImages, new { recipeId = recipeId}).ToList();

                    if (images == null)
                    {
                        return null;
                    }

                    return images;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool InsertImage(int recipeId, Image image)
        {
            string queryInsertImage = @"INSERT INTO [dbo].[Images] ([RecipeId], [FileName], [ContentType], [Data])
                                    VALUES (@recipeId, @fileName, @contentType, @data)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var rowsAffected = connection.Execute(queryInsertImage, new { recipeId, fileName = image.FileName, contentType = image.ContentType, data = image.Data });

                    if (rowsAffected == 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    // Log exception
                    return false;
                }
            }
        }

        public bool UpdateImage(int id, Image image)
        {
            string queryUpdateImage = @"UPDATE [dbo].[Images]
                                        SET [FileName] = @fileName, [ContentType] = @contentType, [Data] = @data
                                        WHERE [Id] = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var rowsAffected = connection.Execute(queryUpdateImage, new { id, fileName = image.FileName, contentType = image.ContentType, data = image.Data });

                    if (rowsAffected == 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    // Log exception
                    return false;
                }
            }
        }

        public bool DeleteImage(int id)
        {
            string queryDeleteImage = @"DELETE FROM [dbo].[Images] WHERE [Id] = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var rowsAffected = connection.Execute(queryDeleteImage, new { id });

                    if (rowsAffected == 0)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    // Log exception
                    return false;
                }
            }
        }
    }
}
