namespace MyPlacesBox.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Models;

    public class HikeScraperService : IHikeScraperService
    {
        private readonly IConfiguration config;
        private readonly IBrowsingContext context;

        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<Region> regionRepository;
        private readonly IDeletableEntityRepository<Town> townRepositiry;
        private readonly IDeletableEntityRepository<Mountain> mountainRepositiry;
        private readonly IRepository<HikeImage> imagesRepository;
        private readonly IDeletableEntityRepository<Hike> hikesRepository;
        private readonly IDeletableEntityRepository<HikeStartPoint> hikeStartPointRepository;
        private readonly IDeletableEntityRepository<HikeEndPoint> hikeEndPointRepository;

        public HikeScraperService(
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<Region> regionRepository,
            IDeletableEntityRepository<Town> townRepositiry,
            IDeletableEntityRepository<Mountain> mountainRepositiry,
            IRepository<HikeImage> imagesRepository,
            IDeletableEntityRepository<Hike> hikesRepository,
            IDeletableEntityRepository<HikeStartPoint> hikeStartPointRepository,
            IDeletableEntityRepository<HikeEndPoint> hikeEndPointRepository)
        {
            this.categoryRepository = categoryRepository;
            this.regionRepository = regionRepository;
            this.townRepositiry = townRepositiry;
            this.mountainRepositiry = mountainRepositiry;
            this.imagesRepository = imagesRepository;
            this.hikesRepository = hikesRepository;
            this.hikeStartPointRepository = hikeStartPointRepository;
            this.hikeEndPointRepository = hikeEndPointRepository;

            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
        }

        public async Task PopulateDbWithHikesAsync()
        {
            var concurentBag = new ConcurrentBag<HikeDto>();

            for (int i = 0; i <= 35; i++)
            {
                var url = new Url($"https://tripsjournal.com/marshruti/page/{i}");

                var hikeDocument = this.context.OpenAsync(url).GetAwaiter().GetResult();

                var allHikes = hikeDocument.QuerySelectorAll("div.list-item");

                foreach (var hike in allHikes)
                {
                    var hikeUrl = hike.QuerySelector("a").GetAttribute("href");

                    try
                    {
                        var hikeDto = this.GetHike(hikeUrl);
                        concurentBag.Add(hikeDto);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach (var hike in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(hike.CategoryName);
                var regionId = await this.GetOrCreateRegionAsync(hike.RegionName);
                var mountainId = await this.GetOrCreateMountainAsync(hike.MountainName);
                var startPointId = await this.GetOrCreateStartPointAsync(
                    hike.HikeStartPointName,
                    hike.HikeStartPointAltitude,
                    hike.HikeStartPointLatitude,
                    hike.HikeStartPointLongitude);
                var endPointId = await this.GetOrCreateEndPointAsync(
                    hike.HikeEndPointName,
                    hike.HikeEndPointAltitude,
                    hike.HikeEndPointLatitude,
                    hike.HikeEndPointLongitude);

                var landmarkExists = this.hikesRepository.AllAsNoTracking().Any(x => x.Name == hike.Name);

                if (landmarkExists)
                {
                    continue;
                }

                Random rnd = new Random();
                int stars = rnd.Next(1, 7);  // creates a number star between 1 and 6
                var newHike = new Hike()
                {
                    Name = hike.Name,
                    CategoryId = categoryId,
                    RegionId = regionId,
                    MountainId = mountainId,
                    HikeStartPointId = startPointId,
                    HikeEndPointId = endPointId,
                    Length = hike.Length,
                    Denivelation = hike.Denivelation,
                    Duration = hike.Duration,
                    Marking = hike.Marking,
                    Difficulty = hike.Difficulty,
                    Description = hike.Description,
                    Stars = stars,
                };

                var imagesCollection = new List<HikeImage>();

                foreach (var image in hike.HikeImages)
                {
                    var imageUrl = this.imagesRepository
                        .AllAsNoTracking()
                        .FirstOrDefault(x => x.RemoteImageUrl == image.RemoteImageUrl);

                    if (imageUrl == null)
                    {
                        imageUrl = new HikeImage
                        {
                            RemoteImageUrl = image.RemoteImageUrl,
                            Extension = image.RemoteImageUrl.Split('.').Last(),
                            Hike = newHike,
                        };

                        await this.imagesRepository.AddAsync(imageUrl);
                        imagesCollection.Add(imageUrl);
                    }
                }

                newHike.HikeImages = imagesCollection;

                await this.hikesRepository.AddAsync(newHike);
                await this.hikesRepository.SaveChangesAsync();
            }
        }

        private async Task<int> GetOrCreateStartPointAsync(
            string hikeStartPointName,
            int hikeStartPointAltitude,
            double hikeStartPointLatitude,
            double hikeStartPointLongitude)
        {
            var startPoint = this.hikeStartPointRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == hikeStartPointName);

            if (startPoint == null)
            {
                startPoint = new HikeStartPoint()
                {
                    Name = hikeStartPointName,
                    Altitude = hikeStartPointAltitude,
                    Latitude = hikeStartPointLatitude,
                    Longitute = hikeStartPointLongitude,
                };

                await this.hikeStartPointRepository.AddAsync(startPoint);
                await this.hikeStartPointRepository.SaveChangesAsync();
            }

            return startPoint.Id;
        }

        private async Task<int> GetOrCreateEndPointAsync(
             string hikeEndPointName,
             int hikeEndPointAltitude,
             double hikeEndPointLatitude,
             double hikeEndPointLongitude)
        {
            var endPoint = this.hikeEndPointRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == hikeEndPointName);

            if (endPoint == null)
            {
                endPoint = new HikeEndPoint()
                {
                    Name = hikeEndPointName,
                    Altitude = hikeEndPointAltitude,
                    Latitude = hikeEndPointLatitude,
                    Longitute = hikeEndPointLongitude,
                };

                await this.hikeEndPointRepository.AddAsync(endPoint);
                await this.hikeEndPointRepository.SaveChangesAsync();
            }

            return endPoint.Id;
        }

        private async Task<int> GetOrCreateMountainAsync(string mountainName)
        {
            if (mountainName == "—")
            {
                var mountain = this.mountainRepositiry
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == "none");

                if (mountain == null)
                {
                    mountain = new Mountain()
                    {
                        Name = "none",
                    };

                    await this.mountainRepositiry.AddAsync(mountain);
                    await this.mountainRepositiry.SaveChangesAsync();
                }

                return mountain.Id;
            }
            else
            {
                var mountain = this.mountainRepositiry
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == mountainName);

                if (mountain == null)
                {
                    mountain = new Mountain()
                    {
                        Name = mountainName,
                    };

                    await this.mountainRepositiry.AddAsync(mountain);
                    await this.mountainRepositiry.SaveChangesAsync();
                }

                return mountain.Id;
            }
        }

        private async Task<int> GetOrCreateTownAsync(string townName)
        {
            if (townName == "—")
            {
                var town = this.townRepositiry
                     .AllAsNoTracking()
                     .FirstOrDefault(x => x.Name == "none");

                if (town == null)
                {
                    town = new Town()
                    {
                        Name = "none",
                        IsTown = false,
                    };

                    await this.townRepositiry.AddAsync(town);
                    await this.townRepositiry.SaveChangesAsync();
                }

                return town.Id;
            }
            else
            {
                var extractedTownName = townName.Split('.');
                var town = this.townRepositiry
                     .AllAsNoTracking()
                     .FirstOrDefault(x => x.Name == extractedTownName[1]);

                if (town == null)
                {
                    town = new Town()
                    {
                        Name = extractedTownName[1],
                        IsTown = extractedTownName[0] == "гр" ? true : false,
                    };

                    await this.townRepositiry.AddAsync(town);
                    await this.townRepositiry.SaveChangesAsync();
                }

                return town.Id;
            }
        }

        private async Task<int> GetOrCreateRegionAsync(string regionName)
        {
            var region = this.regionRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == regionName);

            if (region == null)
            {
                region = new Region()
                {
                    Name = regionName,
                };

                await this.regionRepository.AddAsync(region);
                await this.regionRepository.SaveChangesAsync();
            }

            return region.Id;
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var category = this.categoryRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == categoryName);

            if (category == null)
            {
                category = new Category()
                {
                    Name = categoryName,
                    Type = "Hike",
                };

                await this.categoryRepository.AddAsync(category);
                await this.regionRepository.SaveChangesAsync();
            }

            return category.Id;
        }

        private HikeDto GetHike(string url)
        {
            var document = this.context.OpenAsync(url).GetAwaiter().GetResult();

            if (document.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Not found url({url})");
                throw new InvalidOperationException();
            }

            var hike = new HikeDto();

            // Get name
            var hikeNames = document.QuerySelectorAll(".title > h1");
            foreach (var item in hikeNames)
            {
                var hikeName = item.TextContent;
                hike.Name = hikeName;
            }

            // Get landmark model elements
            var hikeModelElements = document.QuerySelectorAll(".list-table > ul > li > span");

            var categoryName = hikeModelElements[0].TextContent;
            hike.CategoryName = categoryName;

            var regionName = hikeModelElements[2].TextContent;
            hike.RegionName = regionName;

            var mountainName = hikeModelElements[3].TextContent;
            hike.MountainName = mountainName;

            var startPointName = hikeModelElements[4].TextContent.Split("( ");
            hike.HikeStartPointName = startPointName[0];

            var pointCoordinates = document.QuerySelectorAll(".list-table > ul > li > span > small > em");
            var latitudeStartPoint = pointCoordinates[0].TextContent;
            var longitudeStartPoint = pointCoordinates[1].TextContent;
            hike.HikeStartPointLatitude = double.Parse(latitudeStartPoint);
            hike.HikeStartPointLongitude = double.Parse(longitudeStartPoint);

            var endPointName = hikeModelElements[5].TextContent.Split("( ");
            hike.HikeEndPointName = endPointName[0];

            var latitudeEndPoint = pointCoordinates[2].TextContent;
            var longitudeEndPoint = pointCoordinates[3].TextContent;
            hike.HikeEndPointLatitude = double.Parse(latitudeEndPoint);
            hike.HikeEndPointLongitude = double.Parse(longitudeEndPoint);

            var fullDenivelationInfo = hikeModelElements[6].TextContent;
            var denivelationAsString = fullDenivelationInfo.Split(" м");
            int denivelationHike = int.Parse(denivelationAsString[0]);
            hike.Denivelation = denivelationHike;

            int denivelationStartpoint = int.Parse(denivelationAsString[1]);
            var endPointDenivelation = denivelationAsString[2].Split('-');
            int denivelationEndPoint = int.Parse(endPointDenivelation[1].TrimStart());

            hike.HikeStartPointAltitude = denivelationStartpoint;
            hike.HikeEndPointAltitude = denivelationEndPoint;

            var lenght = hikeModelElements[7].TextContent.Split();
            hike.Length = double.Parse(lenght[0]);

            var time = hikeModelElements[8].TextContent.Split();
            var duration = time[0].Split(":");
            int timeInMinutes = (int.Parse(duration[0]) * 60) + int.Parse(duration[1]);
            hike.Duration = TimeSpan.FromMinutes(timeInMinutes);

            var markingAndDificulty = document.QuerySelectorAll(".list-table > ul > li > span > em.rating");
            hike.Marking = int.Parse(markingAndDificulty[0].GetAttribute("data-score"));
            hike.Difficulty = int.Parse(markingAndDificulty[1].GetAttribute("data-score"));

            // Get description
            var hikeDescription = document.QuerySelectorAll(".post > .entry > p");

            StringBuilder sb = new StringBuilder();
            foreach (var item in hikeDescription)
            {
                sb.AppendLine(item.TextContent);
            }

            var allDescription = sb.ToString().TrimEnd();
            hike.Description = allDescription;

            // Get image
          //  var imagesUrl = document.QuerySelectorAll(".gallery-small > div.gallery-small-images > a > img");
            var imagesUrl = document.QuerySelectorAll(".gallery-small > div.gallery-small-images > a");

            foreach (var imageUrl in imagesUrl)
            {
                var curentImageUrl = imageUrl.GetAttribute("href");
                HikeImage image = new HikeImage
                {
                    RemoteImageUrl = curentImageUrl,
                };
                hike.HikeImages.Add(image);
            }

            return hike;
        }
    }
}
