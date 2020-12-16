namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;
    using MyPlacesBox.Web.ViewModels.Landmarks;

    public class LandmarksService : ILandmarksService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };
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
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                WorkTime = input.WorkTime,
                DayOff = input.DayOff,
                EntranceFee = input.EntranceFee,
                Difficulty = input.Difficulty,
                UserId = userId,
            };

            // landmark.Town.IsTown = true;
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

                var physicalPath = $"{imagePath}/landmarks/{dbImage.Id}.{extension}";

                string localImgUrl = physicalPath.Split("wwwroot")[1];
                dbImage.RemoteImageUrl = localImgUrl;
                landmark.LandmarkImages.Add(dbImage);

                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.landmarksRepository.AddAsync(landmark);
            await this.landmarksRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var landmark = this.landmarksRepository.All().FirstOrDefault(x => x.Id == id);
            this.landmarksRepository.Delete(landmark);
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

        public T GetById<T>(int id)
        {
            var landmark = this.landmarksRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return landmark;
        }

        public int GetCount()
        {
          return this.landmarksRepository.All().Count();
        }

        public IEnumerable<T> GetRandom<T>(int count)
        {
            return this.landmarksRepository.All()
                 .OrderBy(x => Guid.NewGuid()) // this return random in EF
                 .Take(count)
                 .To<T>().ToList();
        }

        public async Task UpdateAsync(int id, EditLandmarkInputModel input)
        {
            var landmark = this.landmarksRepository.All().FirstOrDefault(x => x.Id == id);
            landmark.Name = input.Name;
            landmark.CategoryId = input.CategoryId;
            landmark.RegionId = input.RegionId;
            landmark.TownId = input.TownId;
            landmark.MountainId = input.MountainId;
            landmark.Latitude = input.Latitude;
            landmark.Longitute = input.Longitute;
            landmark.Websate = input.Websate;
            landmark.PhoneNumber = input.PhoneNumber;
            landmark.WorkTime = input.WorkTime;
            landmark.DayOff = input.DayOff;
            landmark.EntranceFee = input.EntranceFee;
            landmark.Difficulty = input.Difficulty;

            await this.landmarksRepository.SaveChangesAsync();
        }
    }
}
