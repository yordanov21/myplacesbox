namespace MyPlacesBox.Services.Data
{
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;
    using MyPlacesBox.Web.ViewModels.Landmarks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class LandmarksService : ILandmarksService
    {
        private readonly IDeletableEntityRepository<Landmark> landmarksRepository;
        private readonly IDeletableEntityRepository<LandmarkImage> landmarkImagesRepository;

        public LandmarksService(
            IDeletableEntityRepository<Landmark> landmarksRepository,
            IDeletableEntityRepository<LandmarkImage> landmarkImagesRepository)
        {
            this.landmarksRepository = landmarksRepository;
            this.landmarkImagesRepository = landmarkImagesRepository;
        }

        public async Task CreateAsync(CreateLandmarkInputModel input, string userId)
        {
            var landmark = new Landmark
            {
                Name = input.Name,
                CategoryId = input.CategoryId,
                RegionId = input.RegionId,
                TownId = input.TownId,
                MountainId = input.MountainId,
                Description = input.Description,
                Latitude = input.Latitude,
                Longitute = input.Longitute,
                Websate = input.Websate,
                // TODO Add adress after is iplement in the models and DB
                PhoneNumber = input.PhoneNumber,
                WorkTime = input.WorkTime,
                DayOff = input.DayOff,
                EntranceFee = input.EntranceFee,
                Difficulty = input.Difficulty,
                Stars = input.Stars,
                UserId = userId,
            };

         //   landmark.Town.IsTown = true;
            foreach (var item in input.LandmarkImages)
            {
                var image = this.landmarkImagesRepository.All().FirstOrDefault(x => x.UrlPath == item.UrlPath);

                if (image == null)
                {
                    image = new LandmarkImage { UrlPath = item.UrlPath };
                }

                landmark.LandmarkImages.Add(image);
            }

            await this.landmarksRepository.AddAsync(landmark);
            await this.landmarksRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPage = 10)
        {
            var landmarks = this.landmarksRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .To<T>() // From automapper
                .ToList();

            return landmarks;
        }
    }
}
