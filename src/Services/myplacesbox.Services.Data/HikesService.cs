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
        private readonly IDeletableEntityRepository<HikeImage> imagesRepository;

        public HikesService(
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository,
            IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository,
            IDeletableEntityRepository<HikeImage> imagesRepository)
        {
            this.hikesRepository = hikesRepository;
            this.hikeStartPointsRepository = hikeStartPointsRepository;
            this.hikeEndPointsRepository = hikeEndPointsRepository;
            this.imagesRepository = imagesRepository;
        }

        public async Task CreateAsync(CreateHikeInputModel input, string userId)
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
                    Latitude = input.HikeStartPoint.Longitute,
                    Longitute = input.HikeStartPoint.Longitute,
                };

                await this.hikeStartPointsRepository.AddAsync(startPoint);
                await this.hikeStartPointsRepository.SaveChangesAsync();
            }

            var endPoint = this.hikeEndPointsRepository.All()
                .FirstOrDefault(x => x.Name == input.HikeEndPoint.Name);

            if (endPoint == null)
            {
                endPoint = new HikeEndPoint
                {
                    Name = input.HikeEndPoint.Name,
                    Altitude = input.HikeEndPoint.Altitude,
                    Latitude = input.HikeEndPoint.Latitude,
                    Longitute = input.HikeEndPoint.Longitute,
                };

                await this.hikeEndPointsRepository.AddAsync(endPoint);
                await this.hikeEndPointsRepository.SaveChangesAsync();
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
                RegionId = input.RegionId,
                MountainId = input.RegionId,
                HikeStartPointId = startPoint.Id,
                HikeEndPointId = endPoint.Id,
                Denivelation = startPoint.Altitude - endPoint.Altitude,
                UserId = userId,
            };

            foreach (var item in input.HikeImages)
            {
                var image = this.imagesRepository.All().FirstOrDefault(x => x.UrlPath == item.UrlPath);

                if (image == null)
                {
                    image = new HikeImage { UrlPath = item.UrlPath };
                }

                hike.HikeImages.Add(image);
            }

            await this.hikesRepository.AddAsync(hike);
            await this.hikesRepository.SaveChangesAsync();
        }
    }
}
