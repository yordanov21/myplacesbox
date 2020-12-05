using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public class HikeStartPointsService : IHikeStartPointsService
    {
        private readonly IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository;

        public HikeStartPointsService(IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository)
        {
            this.hikeStartPointsRepository = hikeStartPointsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.hikeStartPointsRepository.AllAsNoTracking()
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
