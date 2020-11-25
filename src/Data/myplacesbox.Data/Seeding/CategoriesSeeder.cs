namespace MyPlacesBox.Data.Seeding
{
    using MyPlacesBox.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Ecotrails", Type = "Hike" });
            await dbContext.Categories.AddAsync(new Category { Name = "Wallking routes", Type = "Hike" });
            await dbContext.Categories.AddAsync(new Category { Name = "Bike routes", Type = "Hike" });

            await dbContext.Categories.AddAsync(new Category { Name = "Natural", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Historical", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Еthnographic", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Мonasteries", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Temples", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Monuments", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Museums", Type = "Landmark" });
            await dbContext.Categories.AddAsync(new Category { Name = "Others", Type = "Landmark" });

            await dbContext.SaveChangesAsync();
            
            // not finished
        }
    }
}
