using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public class TownsService : ITownsService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;

        public TownsService(IDeletableEntityRepository<Town> townsRepository)
        {
            this.townsRepository = townsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.townsRepository.AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.IsTown,
                })
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.IsTown)
                .ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
