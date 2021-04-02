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
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}