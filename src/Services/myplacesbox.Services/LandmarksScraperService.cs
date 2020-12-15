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

    public class LandmarksScraperService : ILandmarksScraperService
    {
        private readonly IConfiguration config;
        private readonly IBrowsingContext context;

        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<Region> regionRepository;
        private readonly IDeletableEntityRepository<Town> townRepositiry;
        private readonly IDeletableEntityRepository<Mountain> mountainRepositiry;
        private readonly IRepository<LandmarkImage> imagesRepository;
        private readonly IDeletableEntityRepository<Landmark> landmarksRepository;

        public LandmarksScraperService(
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<Region> regionRepository,
            IDeletableEntityRepository<Town> townRepositiry,
            IDeletableEntityRepository<Mountain> mountainRepositiry,
            IRepository<LandmarkImage> imagesRepository,
            IDeletableEntityRepository<Landmark> landmarksRepository)
        {
            this.categoryRepository = categoryRepository;
            this.regionRepository = regionRepository;
            this.townRepositiry = townRepositiry;
            this.mountainRepositiry = mountainRepositiry;
            this.imagesRepository = imagesRepository;
            this.landmarksRepository = landmarksRepository;

            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
        }

        public async Task PopulateDbWithLandmarksAsync()
        {
            var concurentBag = new ConcurrentBag<LandmarkDto>();

            for (int i = 0; i <= 15; i++)
            {
                var url = new Url($"https://tripsjournal.com/zabelejitelnosti/page/{i}");

                var landmarkDocument = this.context.OpenAsync(url).GetAwaiter().GetResult();

                var allLandmarks = landmarkDocument.QuerySelectorAll("div.list-item");

                foreach (var landmark in allLandmarks)
                {
                    var landmarkUrl = landmark.QuerySelector("a").GetAttribute("href");
                    Console.WriteLine(landmarkUrl);

                    try
                    {
                        var landmarkDto = this.GetLandmark(landmarkUrl);
                        concurentBag.Add(landmarkDto);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach (var landmark in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(landmark.CategoryName);
                var regionId = await this.GetOrCreateRegionAsync(landmark.RegionName);
                var townId = await this.GetOrCreateTownAsync(landmark.TownName);
                var mountainId = await this.GetOrCreateMountainAsync(landmark.MountainName);
               // var images = await this.GetOrCreateImageAsync(landmark.Images);
                var landmarkExists = this.landmarksRepository.AllAsNoTracking().Any(x => x.Name == landmark.Name);

                if (landmarkExists)
                {
                    continue;
                }

                Random rnd = new Random();
                int stars = rnd.Next(1, 7);  // creates a number star between 1 and 6
                var newLandmark = new Landmark()
                {
                    Name = landmark.Name,
                    CategoryId = categoryId,
                    RegionId = regionId,
                    TownId = townId,
                    MountainId = mountainId,
                    Latitude = landmark.Latitude,
                    Longitute = landmark.Longitute,
                    Websate = landmark.Websate,
                    Address = landmark.Address,
                    PhoneNumber = landmark.PhoneNumber,
                    WorkTime = landmark.WorkTime,
                    DayOff = landmark.DayOff,
                    EntranceFee = landmark.EntranceFee,
                    Difficulty = landmark.Difficulty,
                    Description = landmark.Description,
                    Stars = stars,
                };

                var imagesCollection = new List<LandmarkImage>();

                foreach (var image in landmark.Images)
                {
                    var imageUrl = this.imagesRepository
                        .AllAsNoTracking()
                        .FirstOrDefault(x => x.RemoteImageUrl == image.RemoteImageUrl);

                    if (imageUrl == null)
                    {
                        imageUrl = new LandmarkImage
                        {
                            RemoteImageUrl = image.RemoteImageUrl,
                            Extension = image.RemoteImageUrl.Split('.').Last(),
                            Landmark = newLandmark,
                        };

                        await this.imagesRepository.AddAsync(imageUrl);
                        imagesCollection.Add(imageUrl);
                    }
                }

                newLandmark.LandmarkImages = imagesCollection;

                await this.landmarksRepository.AddAsync(newLandmark);
                await this.landmarksRepository.SaveChangesAsync();
            }
        }

        private async Task<ICollection<LandmarkImage>> GetOrCreateImageAsync(ICollection<LandmarkImage> images)
        {
            var imagesCollection = new List<LandmarkImage>();

            foreach (var image in images)
            {
                var imageUrl = this.imagesRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.RemoteImageUrl == image.RemoteImageUrl);

                if (imageUrl == null)
                {
                    imageUrl = new LandmarkImage
                    {
                        RemoteImageUrl = image.RemoteImageUrl,
                        Extension = image.RemoteImageUrl.Split('.')[1],
                    };

                    await this.imagesRepository.AddAsync(imageUrl);
                    await this.imagesRepository.SaveChangesAsync();
                    imagesCollection.Add(imageUrl);
                }
            }

            return imagesCollection;
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
                    Type = "Landmark",
                };

                await this.categoryRepository.AddAsync(category);
                await this.regionRepository.SaveChangesAsync();
            }

            return category.Id;
        }

        private LandmarkDto GetLandmark(string url)
        {
            var document = this.context.OpenAsync(url).GetAwaiter().GetResult();

            if (document.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Not found url({url})");
                throw new InvalidOperationException();
            }

            var landmark = new LandmarkDto();

            // Get name
            var landmarkNames = document.QuerySelectorAll(".title > h1");
            foreach (var item in landmarkNames)
            {
                var landmarkName = item.TextContent;
                landmark.Name = landmarkName;
            }

            // Get landmark model elements
            var ladmarkModelElements = document.QuerySelectorAll(".list-table > ul > li > span");

            var categoryName = ladmarkModelElements[0].TextContent;
            landmark.CategoryName = categoryName;

            var regionName = ladmarkModelElements[1].TextContent;
            landmark.RegionName = regionName;

            var townName = ladmarkModelElements[2].TextContent;
            landmark.TownName = townName;

            var mountainName = ladmarkModelElements[3].TextContent;
            landmark.MountainName = mountainName;

            var coordinates = ladmarkModelElements[4].TextContent.Split(", ");
            double latitude = double.Parse(coordinates[0]);
            double longitude = double.Parse(coordinates[1]);
            landmark.Latitude = latitude;
            landmark.Longitute = longitude;

            var website = ladmarkModelElements[5].TextContent;
            landmark.Websate = website;

            var address = ladmarkModelElements[6].TextContent;
            landmark.Address = address;

            var phone = ladmarkModelElements[7].TextContent;
            landmark.PhoneNumber = phone;

            var workTime = ladmarkModelElements[8].TextContent;
            landmark.WorkTime = workTime;

            var daysOff = ladmarkModelElements[9].TextContent;
            landmark.DayOff = daysOff;

            var entranceFeeAsString = ladmarkModelElements[10].TextContent.Split(' ');
            double entranceFee = 0;
            bool isThereFee = double.TryParse(entranceFeeAsString[0], out entranceFee);
            landmark.EntranceFee = isThereFee ? entranceFee : 0;

            var accessibility = document.QuerySelectorAll(".list-table > ul > li > span > em.rating");
            foreach (var item in accessibility)
            {
                var currentAccessibility = int.Parse(item.GetAttribute("data-score"));
                landmark.Difficulty = 6 - currentAccessibility;
            }

            // Get description
            var landmarkDescription = document.QuerySelectorAll(".post > .entry > p");

            StringBuilder sb = new StringBuilder();
            foreach (var item in landmarkDescription)
            {
                sb.AppendLine(item.TextContent);
            }

            var allDescripition = sb.ToString().TrimEnd();
            landmark.Description = allDescripition;

            // Get images
            var imagesUrl = document.QuerySelectorAll(".gallery-slides div.gallery-slide");
            foreach (var imageUrl in imagesUrl)
            {
                var curentImageContent = imageUrl.GetAttribute("style");
                var curentImageUrl = curentImageContent.Split("url(")[1];
                curentImageUrl = curentImageUrl.TrimEnd(')');
                LandmarkImage image = new LandmarkImage
                {
                    RemoteImageUrl = curentImageUrl,
                };
                landmark.Images.Add(image);
            }

            return landmark;
        }
    }
}
