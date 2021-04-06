using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence;

namespace Persistence
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext context)
        {
            try
            {
              if (!context.QuestionCategories.Any())
              {
                  var questionCategoriesData = File.ReadAllText("../Persistence/SeedData/questionCategories.json");
                  var questionCategories = JsonSerializer.Deserialize<List<QuestionCategory>>(questionCategoriesData);
                  foreach (var item in questionCategories)
                  {
                      context.QuestionCategories.Add(item);
                  }
                  await context.SaveChangesAsync();
              }
                
              if (!context.InputTypes.Any())
              {
                  var inputTypesData = File.ReadAllText("../Persistence/SeedData/inputTypes.json");
                  var inputTypes = JsonSerializer.Deserialize<List<InputType>>(inputTypesData);
                  foreach (var item in inputTypes)
                  {
                      context.InputTypes.Add(item);
                  }
                  await context.SaveChangesAsync();
              }
              
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}