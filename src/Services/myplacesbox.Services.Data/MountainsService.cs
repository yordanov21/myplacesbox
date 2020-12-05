namespace MyPlacesBox.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class MountainsService : IMountainsService
    {
        private readonly IDeletableEntityRepository<Mountain> mountainsRepository;

        public MountainsService(IDeletableEntityRepository<Mountain> mountainsRepository)
        {
            this.mountainsRepository = mountainsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.mountainsRepository.AllAsNoTracking()
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
