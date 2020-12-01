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
        private readonly IRepository<Image> imagesRepository;
        private readonly IDeletableEntityRepository<Landmark> landmarksRepository;

        public LandmarksScraperService(
            IDeletableEntityRepository<Category> categoryRepository,
            IDeletableEntityRepository<Region> regionRepository,
            IDeletableEntityRepository<Town> townRepositiry,
            IDeletableEntityRepository<Mountain> mountainRepositiry,
            IRepository<Image> imagesRepository,
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

                // Use the default configuration for AngleSharp
              //  var config = Configuration.Default.WithDefaultLoader();
              //  var context = BrowsingContext.New(config);

                var landmarkDocument = this.context.OpenAsync(url).GetAwaiter().GetResult();

                var allLandmarks = landmarkDocument.QuerySelectorAll("div.list-item");

                // Console.WriteLine("index = " + i);
                int count = 0;
                foreach (var landmark in allLandmarks)
                {
                    //Console.WriteLine("counter for landmark: " + ++count);
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

                   // landMarksCounter++;
                   // Console.WriteLine("Landmarks counter: " + landMarksCounter);
                }
            }

            foreach (var landmark in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(landmark.CategoryName);
                var regionId = await this.GetOrCreateRegionAsync(landmark.RegionName);
                var townId = await this.GetOrCreateTownAsync(landmark.TownName);
                var mountainId = await this.GetOrCreateMountainAsync(landmark.MountainName);

                var images = await this.GetOrCreateImageAsync(landmark.Images);

                var landmarkExists = this.landmarksRepository.AllAsNoTracking().Any(x => x.Name == landmark.Name);

                if( landmarkExists)
                {
                    continue;
                }

                Random rnd = new Random();
                int stars = rnd.Next(1, 7);  // creates a number between 1 and 6
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
                    // Address = landmark.Address //TODO add adress in DB landmark table
                    PhoneNumber = landmark.PhoneNumber,
                    WorkTime = landmark.WorkTime,
                    DayOff = landmark.DayOff,
                    EntranceFee = landmark.EntranceFee,
                    Difficulty = landmark.Difficulty,
                    Description = landmark.Description,
                    Stars = stars,
                    Images = images,

                };

            }

            await this.categoryRepository.SaveChangesAsync();
            await this.regionRepository.SaveChangesAsync();
            await this.townRepositiry.SaveChangesAsync();
            await this.mountainRepositiry.SaveChangesAsync();
            await this.imagesRepository.SaveChangesAsync();
            await this.landmarksRepository.SaveChangesAsync();
        }

        private async Task<ICollection<Image>> GetOrCreateImageAsync(ICollection<Image> images)
        {
            var imagesCollection = new List<Image>();

            foreach (var image in images)
            {
                var imageUrl = this.imagesRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Extension == image.Extension);
                   
                if (imageUrl == null)
                {
                    imageUrl = new Image
                    {
                        Extension = imageUrl.Extension,
                        LandmarkId = imageUrl.LandmarkId
                    };

                    await this.imagesRepository.AddAsync(imageUrl);
                    imagesCollection.Add(imageUrl);
                }
            }

            return imagesCollection;
        }

        private async Task<int> GetOrCreateMountainAsync(string mountainName)
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
            }

            return mountain.Id;
        }

        private async Task<int> GetOrCreateTownAsync(string townName)
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
            }

            return town.Id;
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

                // Console.WriteLine("Name: " + landmarkName);
            }

            // Get landmark model elements
            var ladmarkModelElements = document.QuerySelectorAll(".list-table > ul > li > span");

            var categoryName = ladmarkModelElements[0].TextContent;
            landmark.CategoryName = categoryName;
            // Console.WriteLine("Category: " + categoryName);

            var regionName = ladmarkModelElements[1].TextContent;
            landmark.RegionName = regionName;
            // Console.WriteLine("Region: " + regionName);

            var townName = ladmarkModelElements[2].TextContent;
            landmark.TownName = townName;
            // Console.WriteLine("Town: " + townName);

            var mountainName = ladmarkModelElements[3].TextContent;
            landmark.MountainName = mountainName;
            // Console.WriteLine("Mountain: " + mountainName);

            var coordinates = ladmarkModelElements[4].TextContent.Split(", ");
            double latitude = double.Parse(coordinates[0]);
            double longitude = double.Parse(coordinates[1]);
            landmark.Latitude = latitude;
            landmark.Longitute = longitude;
           // Console.WriteLine("Coordinates: " + coordinates);

            var website = ladmarkModelElements[5].TextContent;
            landmark.Websate = website;
           // Console.WriteLine("Website: " + website);

            var address = ladmarkModelElements[6].TextContent;
            landmark.Address = address;
            // Console.WriteLine("Address: " + address);

            var phone = ladmarkModelElements[7].TextContent;
            landmark.PhoneNumber = phone;
          //  Console.WriteLine("Phone: " + phone);

            var workTime = ladmarkModelElements[8].TextContent;
            landmark.WorkTime = workTime;
          //  Console.WriteLine("Worktime: " + workTime);

            var daysOff = ladmarkModelElements[9].TextContent;
            landmark.DayOff = daysOff;
            // Console.WriteLine("DaysOff: " + daysOff);

            var entranceFeeAsString = ladmarkModelElements[10].TextContent.Split(' ');
            double entranceFee = 0;
            bool isThereFee = double.TryParse(entranceFeeAsString[0], out entranceFee);
            landmark.EntranceFee = isThereFee ? entranceFee : 0;
            //  Console.WriteLine("EntranceFee: " + entranceFeeAsString);

            var accessibility = document.QuerySelectorAll(".list-table > ul > li > span > em.rating");
            foreach (var item in accessibility)
            {
                var currentAccessibility = int.Parse(item.GetAttribute("data-score"));
                landmark.Difficulty = (6 - currentAccessibility);
              //  Console.WriteLine("Accessibility: " + currentAccessibility);
            }

            // get description
            var landmarkDescription = document.QuerySelectorAll(".post > .entry > p");

            StringBuilder sb = new StringBuilder();
            foreach (var item in landmarkDescription)
            {
                sb.AppendLine(item.TextContent);
                // Console.WriteLine(item.TextContent); 
            }

            var allDescripition = sb.ToString().TrimEnd();
            landmark.Description = allDescripition;

            // get images
           
            var imagesUrl = document.QuerySelectorAll(".gallery-slides div.gallery-slide");
            foreach (var imageUrl in imagesUrl)
            {
                var curentImageContent = imageUrl.GetAttribute("style");
                var curentImageUrl = curentImageContent.Split("url(")[1];
                curentImageUrl = curentImageUrl.TrimEnd(')');
                Image image = new Image();
                image.Extension = curentImageUrl;
                landmark.Images.Add(image);
              //  Console.WriteLine("Image url: " + curentImageUrl);
            }

            return landmark;
        }
    }
}
