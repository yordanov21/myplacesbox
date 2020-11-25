namespace MyPlacesBox.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
           return this.categoriesRepository.AllAsNoTracking()  //is better from All() pestime malko panmet, kogato durpame danni e po dobre da e s AllAsNoTraking
                .Where(c => c.Type == "Hike")
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList()
                  .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
