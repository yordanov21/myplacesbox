namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;
    using MyPlacesBox.Web.ViewModels.Landmarks;

    public class LandmarksService : ILandmarksService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Landmark> landmarksRepository;

        public LandmarksService(
            IDeletableEntityRepository<Landmark> landmarksRepository)
        {
            this.landmarksRepository = landmarksRepository;
        }

        public async Task CreateAsync(CreateLandmarkInputModel input, string userId, string imagePath)
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

            Directory.CreateDirectory($"{imagePath}/landmarks/");
            foreach (var image in input.LandmarkImages)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new LandmarkImage
                {
                    UserId = userId,
                    Extension = extension,
                };
                landmark.LandmarkImages.Add(dbImage);

                var physicalPath = $"{imagePath}/landmarks/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
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

        public int GetCount()
        {
          return this.landmarksRepository.All().Count();
        }
    }
}
