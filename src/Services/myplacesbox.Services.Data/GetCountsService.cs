namespace MyPlacesBox.Services.Data
{
    using System.Linq;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Data.Models;
    using MyPlacesBox.Web.ViewModels.Home;

    public class GetCountsService : IGetCountsService
    {
        private readonly IDeletableEntityRepository<Landmark> landmarksRepository;
        private readonly IDeletableEntityRepository<Hike> hikesRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Region> regionsRepository;
        private readonly IDeletableEntityRepository<Town> townsRepository;
        private readonly IDeletableEntityRepository<Mountain> mountainsRepository;
        private readonly IDeletableEntityRepository<LandmarkImage> landmarkImagesRepository;
        private readonly IDeletableEntityRepository<HikeImage> hikeImagesRepository;

        public GetCountsService(
            IDeletableEntityRepository<Landmark> landmarksRepository,
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Region> regionsRepository,
            IDeletableEntityRepository<Town> townsRepository,
            IDeletableEntityRepository<Mountain> mountainsRepository,
            IDeletableEntityRepository<LandmarkImage> landmarkImagesRepository,
            IDeletableEntityRepository<HikeImage> hikeImagesRepository)
        {
            this.landmarksRepository = landmarksRepository;
            this.hikesRepository = hikesRepository;
            this.categoriesRepository = categoriesRepository;
            this.regionsRepository = regionsRepository;
            this.townsRepository = townsRepository;
            this.mountainsRepository = mountainsRepository;
            this.landmarkImagesRepository = landmarkImagesRepository;
            this.hikeImagesRepository = hikeImagesRepository;
        }

        public CountsDto GetCounts()
        {
            var data = new CountsDto
            {
                LandmarksCount = this.landmarksRepository.All().Count(),
                LandmarkImagesCount = this.landmarkImagesRepository.All().Count(),
                HikesCount = this.hikesRepository.All().Count(),
                HikeImagesCount = this.hikeImagesRepository.All().Count(),
                CategoriesCount = this.categoriesRepository.All().Count(),
                RegionsCount = this.regionsRepository.All().Count(),
                TownsCount = this.townsRepository.All().Count(),
                MountainsCount = this.mountainsRepository.All().Count(),
            };

            return data;
        }
    }
}
