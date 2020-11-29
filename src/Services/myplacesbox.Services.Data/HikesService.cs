using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using MyPlacesBox.Web.ViewModels.Hikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlacesBox.Services.Data
{
    public class HikesService : IHikesService
    {
        private readonly IDeletableEntityRepository<Hike> hikesRepository;
        private readonly IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository;
        private readonly IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository;
        private readonly IDeletableEntityRepository<Region> regionsRepository;
        private readonly IDeletableEntityRepository<Town> townsRepository;
        private readonly IDeletableEntityRepository<Mountain> mountainsRepository;

        public HikesService(
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository,
            IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository,
            IDeletableEntityRepository<Region> regionsRepository,
            IDeletableEntityRepository<Town> townsRepository,
            IDeletableEntityRepository<Mountain> mountainsRepository)
        {
            this.hikesRepository = hikesRepository;
            this.hikeStartPointsRepository = hikeStartPointsRepository;
            this.hikeEndPointsRepository = hikeEndPointsRepository;
            this.regionsRepository = regionsRepository;
            this.townsRepository = townsRepository;
            this.mountainsRepository = mountainsRepository;
        }

        public async Task CreateAsync(CreateHikeInputModel input)
        {
           
            var startPoint = this.hikeStartPointsRepository.All().FirstOrDefault(x => x.Name == input.HikeStartPoint.Name);
            // TODO sled kato opravq v bazata danni za nadmoskatat visochina i koordinatite da go opravq tuk
            if (startPoint == null)
            {
                startPoint = new HikeStartPoint
                {
                    Name = input.HikeStartPoint.Name,
                    StatrtPointAltitude = 100,
                };

                await this.hikeStartPointsRepository.AddAsync(startPoint);
                await this.hikeStartPointsRepository.SaveChangesAsync();
            }

            var endPoint = this.hikeEndPointsRepository.All().FirstOrDefault(x => x.Name == input.HikeEndPoint.Name);

            if (endPoint == null)
            {
                endPoint = new HikeEndPoint
                {
                    Name = input.HikeStartPoint.Name,
                    StatrtPointAltitude = 20,
                };

                await this.hikeEndPointsRepository.AddAsync(endPoint);
                await this.hikeEndPointsRepository.SaveChangesAsync();
            }

            var region = this.regionsRepository.All().FirstOrDefault(x => x.Name == input.Region.Name);
            if( region == null)
            {
                region = new Region
                {
                    Name = input.Region.Name
                };

                await this.regionsRepository.AddAsync(region);
                await this.regionsRepository.SaveChangesAsync();
            }

            var town = this.townsRepository.All().FirstOrDefault(x => x.Name == input.Town.Name);
            if (town == null)
            {
                town = new Town
                {
                    Name = input.Town.Name
                };

                await this.townsRepository.AddAsync(town);
                await this.townsRepository.SaveChangesAsync();
            }

            var mountain = this.mountainsRepository.All().FirstOrDefault(x => x.Name == input.Region.Name);
            if (mountain == null)
            {
                mountain = new Mountain
                {
                    Name = input.Mountain.Name
                };

                await this.mountainsRepository.AddAsync(mountain);
                await this.mountainsRepository.SaveChangesAsync();
            }

            var hike = new Hike
            {
                Name = input.Name,
                Length = input.Length,
                Duration = TimeSpan.FromMinutes(input.Duration),
                Description = input.Description,
                Marking = input.Marking,
                Difficulty = input.Difficulty,
                Stars = input.Stars,
                CategoryId = input.CategoryId,
                RegionId = region.Id,
                TownId = town.Id,
                MountainId = mountain.Id,
                HikeStartPointId = startPoint.Id,
                HikeEndPointId = endPoint.Id,
                Denivelation = startPoint.StatrtPointAltitude - endPoint.StatrtPointAltitude,              
            };

       
            await this.hikesRepository.AddAsync(hike);
            await this.hikesRepository.SaveChangesAsync();

        }
    }
}
