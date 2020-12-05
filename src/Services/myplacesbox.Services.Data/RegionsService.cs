namespace MyPlacesBox.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class RegionsService : IRegionsService
    {
        private readonly IDeletableEntityRepository<Region> regionsRepository;

        public RegionsService(IDeletableEntityRepository<Region> regionsRepository)
        {
            this.regionsRepository = regionsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.regionsRepository.AllAsNoTracking()
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                })
                .OrderBy(r => r.Name)
                .ToList()
                .Select(r => new KeyValuePair<string, string>(r.Id.ToString(), r.Name));
        }
    }
}
