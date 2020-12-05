namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Web.ViewModels.Hikes;

    public class HikesService : IHikesService
    {
        private readonly IDeletableEntityRepository<Hike> hikesRepository;
        private readonly IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository;
        private readonly IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository;
     //   private readonly IDeletableEntityRepository<Region> regionsRepository;
    //    private readonly IDeletableEntityRepository<Mountain> mountainsRepository;
     //   private readonly IDeletableEntityRepository<HikeImage> imagesRepository;


        public HikesService(
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository,
            IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository,
        //    IDeletableEntityRepository<Region> regionsRepository,
        //    IDeletableEntityRepository<Mountain> mountainsRepository,
            IDeletableEntityRepository<HikeImage> imagesRepository)
        {
            this.hikesRepository = hikesRepository;
            this.hikeStartPointsRepository = hikeStartPointsRepository;
            this.hikeEndPointsRepository = hikeEndPointsRepository;
       //     this.regionsRepository = regionsRepository;
       //     this.mountainsRepository = mountainsRepository;
       //     this.imagesRepository = imagesRepository;
        }

        public async Task CreateAsync(CreateHikeInputModel input)
        {
            var startPoint = this.hikeStartPointsRepository.All()
                .FirstOrDefault(x => x.Name == input.HikeStartPoint.Name);
            // TODO sled kato opravq v bazata danni za nadmoskatat visochina i koordinatite da go opravq tuk
            if (startPoint == null)
            {
                startPoint = new HikeStartPoint
                {
                    Name = input.HikeStartPoint.Name,
                    Altitude = input.HikeStartPoint.Altitude,
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
                    Altitude = 20,
                };

                await this.hikeEndPointsRepository.AddAsync(endPoint);
                await this.hikeEndPointsRepository.SaveChangesAsync();
            }

            //var region = this.regionsRepository.All().FirstOrDefault(x => x.Name == input.Region.Name);
            //if (region == null)
            //{
            //    region = new Region
            //    {
            //        Name = input.Region.Name,
            //    };

            //    await this.regionsRepository.AddAsync(region);
            //    await this.regionsRepository.SaveChangesAsync();
            //}

            //var mountain = this.mountainsRepository.All().FirstOrDefault(x => x.Name == input.Region.Name);
            //if (mountain == null)
            //{
            //    mountain = new Mountain
            //    {
            //        Name = input.Mountain.Name,
            //    };

            //    await this.mountainsRepository.AddAsync(mountain);
            //    await this.mountainsRepository.SaveChangesAsync();
            //}

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
                RegionId = input.RegionId,
                MountainId = input.RegionId,
                HikeStartPointId = startPoint.Id,
                HikeEndPointId = endPoint.Id,
                Denivelation = startPoint.Altitude - endPoint.Altitude,
            };

            await this.hikesRepository.AddAsync(hike);
            await this.hikesRepository.SaveChangesAsync();
        }
    }
}
