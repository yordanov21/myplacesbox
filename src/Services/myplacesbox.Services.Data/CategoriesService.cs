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

        public IEnumerable<KeyValuePair<string, string>> GetAllHikeCategotiesAsKeyValuePairs()
        {
            return this.categoriesRepository.AllAsNoTracking()
               .Where(c => c.Type == "Hike")
               .Select(x => new
               {
                   x.Id,
                   x.Name,
               })
               .OrderBy(x => x.Name)
               .ToList()
               .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllLandmarkCategotiesAsKeyValuePairs()
        {
            return this.categoriesRepository.AllAsNoTracking()
               .Where(c => c.Type == "Landmark")
               .Select(x => new
               {
                   x.Id,
                   x.Name,
               })
               .OrderBy(x => x.Name)
               .ToList()
               .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
