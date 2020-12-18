namespace MyPlacesBox.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Models;
    using Newtonsoft.Json;

    public class TownsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            // Check if there is town in DB, don't add categories in DB
            if (dbContext.Towns.Any())
            {
                return;
            }

            string inputJson = File.ReadAllText("E:\\03_ASP.NET PROJECTS\\myplacesbox\\src\\Data\\MyPlacesBox.Data\\Seeding\\TownsList.json");
            var towns = JsonConvert.DeserializeObject<List<JsonsTownInputModel>>(inputJson);

            foreach (var item in towns)
            {
                await dbContext.Towns.AddAsync(new Town { Name = item.name, IsTown = true });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
