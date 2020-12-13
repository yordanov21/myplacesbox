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
    using MyPlacesBox.Web.ViewModels.Hikes;

    public class HikesService : IHikesService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };
        private readonly IDeletableEntityRepository<Hike> hikesRepository;
        private readonly IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository;
        private readonly IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository;

        public HikesService(
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<HikeStartPoint> hikeStartPointsRepository,
            IDeletableEntityRepository<HikeEndPoint> hikeEndPointsRepository)
        {
            this.hikesRepository = hikesRepository;
            this.hikeStartPointsRepository = hikeStartPointsRepository;
            this.hikeEndPointsRepository = hikeEndPointsRepository;
        }

        public async Task CreateAsync(CreateHikeInputModel input, string userId, string imagePath)
        {
            var startPoint = this.hikeStartPointsRepository.All()
                .FirstOrDefault(x => x.Name == input.HikeStartPoint.Name);

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

            Directory.CreateDirectory($"{imagePath}/hikes/");
            foreach (var image in input.HikeImages)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new HikeImage
                {
                    UserId = userId,
                    Extension = extension,
                };
                hike.HikeImages.Add(dbImage);

                var physicalPath = $"{imagePath}/landmarks/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.hikesRepository.AddAsync(hike);
            await this.hikesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hike = this.hikesRepository.All().FirstOrDefault(x => x.Id == id);
            this.hikesRepository.Delete(hike);
            await this.hikesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPage = 10)
        {
            var landmarks = this.hikesRepository
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
            var hike = this.hikesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return hike;
        }

        public int GetCount()
        {
            return this.hikesRepository.All().Count();
        }

        public IEnumerable<T> GetRandom<T>(int count)
        {
            return this.hikesRepository.All()
                 .OrderBy(x => Guid.NewGuid()) // this return random in EF
                 .Take(count)
                 .To<T>().ToList();
        }

        public async Task UpdateAsync(int id, EditHikekInputModel input)
        {
            ;
            //var startPoint = this.hikeStartPointsRepository.All()
            //      .FirstOrDefault(x => x.Name == input.HikeStartPoint.Name);

            //if (startPoint == null)
            //{
            //    startPoint = new HikeStartPoint
            //    {
            //        Name = input.HikeStartPoint.Name,
            //        Altitude = input.HikeStartPoint.Altitude,
            //        Latitude = input.HikeStartPoint.Longitute,
            //        Longitute = input.HikeStartPoint.Longitute,
            //    };

            //    await this.hikeStartPointsRepository.AddAsync(startPoint);
            //    await this.hikeStartPointsRepository.SaveChangesAsync();
            //}

            //var endPoint = this.hikeEndPointsRepository.All()
            //    .FirstOrDefault(x => x.Name == input.HikeEndPoint.Name);

            //if (endPoint == null)
            //{
            //    endPoint = new HikeEndPoint
            //    {
            //        Name = input.HikeEndPoint.Name,
            //        Altitude = input.HikeEndPoint.Altitude,
            //        Latitude = input.HikeEndPoint.Latitude,
            //        Longitute = input.HikeEndPoint.Longitute,
            //    };

            //    await this.hikeEndPointsRepository.AddAsync(endPoint);
            //    await this.hikeEndPointsRepository.SaveChangesAsync();
            //}

            var hike = this.hikesRepository.All().FirstOrDefault(x => x.Id == id);
      
                hike.Name = input.Name;
            //    Length = input.Length,
            //   Duration = TimeSpan.FromMinutes(input.Duration),
            hike.Description = input.Description;
                //   Marking = input.Marking,
                hike.Difficulty = input.Difficulty;
                hike.Stars = input.Stars;
                hike.CategoryId = input.CategoryId;
                hike.RegionId = input.RegionId;
                hike.MountainId = input.RegionId;
             //   HikeStartPointId = startPoint.Id,
            //    HikeEndPointId = endPoint.Id,
             //   Denivelation = startPoint.Altitude - endPoint.Altitude,
            

            await this.hikesRepository.SaveChangesAsync();
        }
    }
}
