namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;

    public class HikeEndPointsService : IHikeEndPointsService
    {
        private readonly IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository;

        public HikeEndPointsService(IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository)
        {
            this.hikeEndPointsRepository = hikeEndPointsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.hikeEndPointsRepository.AllAsNoTracking()
                 .Select(x => new
                 {
                     x.Id,
                     x.Name,
                     x.Altitude,
                     x.Latitude,
                     x.Longitute,
                 })
                 .OrderBy(x => x.Name)
                 .ToList()
                 .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
