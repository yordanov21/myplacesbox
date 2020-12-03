using AngleSharp;
using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using MyPlacesBox.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPlacesBox.Services
{
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

                //Use the default configuration for AngleSharp
                // var config = Configuration.Default.WithDefaultLoader();
                // var context = BrowsingContext.New(config);

                var hikeDocument = context.OpenAsync(url).GetAwaiter().GetResult();

                var allHikes = hikeDocument.QuerySelectorAll("div.list-item");
                //Console.WriteLine("index = " + i);
                int count = 0;
                foreach (var hike in allHikes)
                {
                    //Console.WriteLine("counter for landmark: " + ++count);
                    var hikeUrl = hike.QuerySelector("a").GetAttribute("href");
                    //   Console.WriteLine(hikeUrl);

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
                var townId = await this.GetOrCreateTownAsync(hike.TownName);
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

                var images = await this.GetOrCreateImageAsync(hike.HikeImages);

                var landmarkExists = this.hikesRepository.AllAsNoTracking().Any(x => x.Name == hike.Name);

                if (landmarkExists)
                {
                    continue;
                }

                Random rnd = new Random();
                int stars = rnd.Next(1, 7);  // creates a number between 1 and 6
                var newLandmark = new Hike()
                {
                    Name = hike.Name,
                    CategoryId = categoryId,
                    RegionId = regionId,
                    TownId = townId,
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
                    HikeImages = images,
                };

                await this.hikesRepository.AddAsync(newLandmark);
                await this.hikesRepository.SaveChangesAsync();
            }
        }

        private async Task<int> GetOrCreateStartPointAsync(
            string hikeStartPointName,
            int hikeStartPointAltitude,
            double hikeStartPointLatitude,
            double HikeStartPointLongitude)
        {
   
            var startPoint = this.hikeStartPointRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == hikeStartPointName);

            if (startPoint == null)
            {
                startPoint = new HikeStartPoint()
                {
                    Name = hikeStartPointName,
                    StatrtPointAltitude = hikeStartPointAltitude,
                    StartPointLatitude = hikeStartPointLatitude,
                    StartPointLongitute = HikeStartPointLongitude
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
             double HikeEndPointLongitude)
        {

            var endPoint = this.hikeEndPointRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == hikeEndPointName);

            if (endPoint == null)
            {
                endPoint = new HikeEndPoint()
                {
                    Name = hikeEndPointName,
                    StatrtPointAltitude = hikeEndPointAltitude,
                    StartPointLatitude = hikeEndPointLatitude,
                    StartPointLongitute = HikeEndPointLongitude
                };

                await this.hikeEndPointRepository.AddAsync(endPoint);
                await this.hikeEndPointRepository.SaveChangesAsync();
            }

            return endPoint.Id;
        }

        private async Task<ICollection<HikeImage>> GetOrCreateImageAsync(ICollection<HikeImage> images)
        {
            var imagesCollection = new List<HikeImage>();

            foreach (var image in images)
            {
                var imageUrl = this.imagesRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.UrlPath == image.UrlPath);

                if (imageUrl == null)
                {
                    imageUrl = new HikeImage
                    {
                        UrlPath = image.UrlPath,
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
                .FirstOrDefault(x => x.Name == mountainName);

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
                     .FirstOrDefault(x => x.Name == townName);

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
            // Get Landmarks elements 

            if (document.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Not found url({url})");
                throw new InvalidOperationException();
            }

            var hike = new HikeDto();

            // get name
            var hikeNames = document.QuerySelectorAll(".title > h1");
            foreach (var item in hikeNames)
            {
                var hikeName = item.TextContent;
                hike.Name = hikeName;
                // Console.WriteLine("Name: " + item.TextContent);
            }

            // get landmark model elements
            var hikeModelElements = document.QuerySelectorAll(".list-table > ul > li > span");

            var categoryName = hikeModelElements[0].TextContent;
            hike.CategoryName = categoryName;
            //Console.WriteLine("Category: " + categoryName);

            //  var seasonName = document.QuerySelector(".list-table > ul > li > span.seasons").GetAttribute("title");
            // Console.WriteLine("Season: " + seasonName);

            var regionName = hikeModelElements[2].TextContent;
            hike.RegionName = regionName;
            // Console.WriteLine("Region: " + regionName);

            var mountainName = hikeModelElements[3].TextContent;
            hike.MountainName = mountainName;
            // Console.WriteLine("Mountain: " + mountainName);

            var startPointName = hikeModelElements[4].TextContent;
            hike.HikeStartPointName = startPointName;
            // Console.WriteLine("StartPoint: " + startPointName);

            var PointCoordinates = document.QuerySelectorAll(".list-table > ul > li > span > small > em");
            var latitudeStartPoint = PointCoordinates[0].TextContent;
            var longitudeStartPoint = PointCoordinates[1].TextContent;
            hike.HikeStartPointLatitude = double.Parse(latitudeStartPoint);
            hike.HikeStartPointLongitude = double.Parse(longitudeStartPoint);
          //  Console.WriteLine("latitudeStartPoint: " + latitudeStartPoint);
           // Console.WriteLine("longitudeStartPoint: " + longitudeStartPoint);

            var endPointName = hikeModelElements[5].TextContent;
            hike.HikeEndPointName = endPointName;
            //  Console.WriteLine("EndPoint: " + endPointName);

            var latitudeEndPoint = PointCoordinates[2].TextContent;
            var longitudeEndPoint = PointCoordinates[3].TextContent;
            hike.HikeEndPointLatitude = double.Parse(latitudeEndPoint);
            hike.HikeEndPointLongitude = double.Parse(longitudeEndPoint);
         //   Console.WriteLine("latitudeStartPoint: " + latitudeEndPoint);
          //  Console.WriteLine("longitudeStartPoint: " + longitudeEndPoint);

            var fullDenivelationInfo = hikeModelElements[6].TextContent;
            var denivelationAsString = fullDenivelationInfo.Split(" м");
            int denivelationHike = int.Parse(denivelationAsString[0]);
            hike.Denivelation = denivelationHike;

            var denivelationPoints = denivelationAsString[1].Split(" - ");
            for (int i = 0; i < denivelationPoints.Length; i++)
            {
                denivelationPoints[i] = denivelationPoints[i].TrimEnd('м');
                denivelationPoints[i] = denivelationPoints[i].TrimEnd();
            }
        
            int denivelationStartpoint = int.Parse(denivelationPoints[0]);
            int denivelationEndPoint = int.Parse(denivelationPoints[1]);

            hike.HikeStartPointAltitude = denivelationStartpoint;
            hike.HikeEndPointAltitude = denivelationEndPoint;

            //TODO go to start end edd point!!!!!!!!!!!!!!!!!!
            //  Console.WriteLine("Denivelation: " + fullDenivelationInfo);
            var denivelationsStartAndPoints = document.QuerySelector(".list-table > ul > li:nth-child(7) > span > small");
            //  Console.WriteLine("denivelationsStartAndPoints: " + denivelationsStartAndPoints);


            var lenght = hikeModelElements[7].TextContent.Split();
            hike.Length = double.Parse(lenght[0]);
            //  Console.WriteLine("lenght: " + lenght);

            var time = hikeModelElements[8].TextContent.Split();
            var duration = time[0].Split(":");
            int timeInMinutes = int.Parse(duration[0]) * 60 + int.Parse(duration[1]);
            hike.Duration = TimeSpan.FromMinutes(timeInMinutes);
        
          //  Console.WriteLine("time: " + time);

            var markingAndDificulty = document.QuerySelectorAll(".list-table > ul > li > span > em.rating");
            hike.Marking = int.Parse(markingAndDificulty[0].GetAttribute("data-score"));
            hike.Difficulty = int.Parse(markingAndDificulty[1].GetAttribute("data-score"));
            //foreach (var item in markingAndDificulty)
            //{
            //    hike.Marking = int.Parse(item.TextContent);
            //    //Console.WriteLine("item: " + item.GetAttribute("data-score"));
            //}

            // get description
            var hikeDescription = document.QuerySelectorAll(".post > .entry > p");

            StringBuilder sb = new StringBuilder();           
            foreach (var item in hikeDescription)
            {
                sb.AppendLine(item.TextContent);
                //  Console.WriteLine(item.TextContent);
               
            }

            var allDescription = sb.ToString().TrimEnd();
            hike.Description = allDescription;
          //  Console.WriteLine("Description paragraph count: " + countDescrioptionParagraf);


            // get image 
            //TODO make it for all images (now is only for one)
            var imagesUrl = document.QuerySelectorAll(".gallery-small > div.gallery-small-images > a > img");
            foreach (var imageUrl in imagesUrl)
            {
                var curentImageUrl = imageUrl.GetAttribute("src");
                HikeImage image = new HikeImage();
                image.UrlPath = curentImageUrl;
                hike.HikeImages.Add(image);
              //  Console.WriteLine("curentImageUrl url: " + curentImageUrl);
            }

            return hike;
        }
    }
}
