namespace MyPlacesBox.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Moq;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Web.ViewModels.Landmarks;
    using Xunit;

    public class LandmarksServiceTest
    {
        //[Fact]
        //public async Task GetCountShouldBeCorrect()
        //{
        //    var list = new List<Landmark>();
        //    var mockRepo = new Mock<IDeletableEntityRepository<Landmark>>();
        //    mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
        //    mockRepo.Setup(x => x.AddAsync(It.IsAny<Landmark>())).Callback(
        //        (Landmark landmark) => list.Add(landmark));
        //    var service = new LandmarksService(mockRepo.Object);

        //    var fileName = "test.png";
        //    var ms = new MemoryStream();
        //    var fileMock = new Mock<IFormFile>();
        //    fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        //    fileMock.Setup(x => x.FileName).Returns(fileName);
        //    fileMock.Setup(x => x.Length).Returns(ms.Length);

        //    var file = fileMock.Object;

       
        //    var user = new ApplicationUser
        //    {
        //        UserName = "user1",
        //        Email = "dada@abv.bg",
        //    };
        //    var images = new List<string>();
        //    images.Add("image.png");
        //    var landmark = new CreateLandmarkInputModel
        //    {
        //        Name = "landmark",
        //        CategoryId = 1,
        //        RegionId = 1,
        //        TownId = 1,
        //        MountainId = 1,
        //        Description = "description ,description description description description description description description description description description description description description description description description description description description description description description description description description description descriptiondescription description description description description description description description description description description description description description description description description  ",
        //        Latitude = 23.235,
        //        Longitute = 43.523,
        //        Websate = "www.abvg.bg",
        //        Address = "Address 1",
        //        PhoneNumber = "0888888888",
        //        WorkTime = "8:00-20:00",
        //        DayOff = "none",
        //        EntranceFee = 5,
        //        Difficulty = 4,
        //        LandmarkImages = (IEnumerable<IFormFile>)file,
        //    };

        //    await service.CreateAsync(landmark, user.Id, "imagepath");

        //    mockRepo.Verify(x => x.AddAsync(It.IsAny<Landmark>()), Times.Exactly(1));

        //    Assert.Equal(1, service.GetCount());
        //}

        [Fact]
        public async Task DeleteAsyncLandmark()
        {
            var list = new List<Landmark>();
            var mockRepo = new Mock<IDeletableEntityRepository<Landmark>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Landmark>()));
            mockRepo.Setup(x => x.Delete(It.IsAny<Landmark>()))
                  .Callback(
                  (Landmark pet) => list.Remove(pet));
            var service = new LandmarksService(mockRepo.Object);

            var user = new ApplicationUser
            {
                UserName = "user1",
                Email = "dada@abv.bg",
            };
            var landmark = new Landmark
            {
                Name = "landmark",
                CategoryId = 1,
                RegionId = 1,
                TownId = 1,
                MountainId = 1,
                Description = "description ,description description description description description description description description description description description description description description description description description description description description description description description description description description descriptiondescription description description description description description description description description description description description description description description description description  ",
                Latitude = 23.235,
                Longitute = 43.523,
                Websate = "www.abvg.bg",
                Address = "Address 1",
                PhoneNumber = "0888888888",
                WorkTime = "8:00-20:00",
                DayOff = "none",
                EntranceFee = 5,
                Difficulty = 4,
                UserId = user.Id,
            };

            list.Add(landmark);

            await service.DeleteAsync(landmark.Id);

            Assert.Equal(0, service.GetCount());
        }

        //[Fact]
        //public async Task MethodShouldReturnGetById()
        //{
        //    var list = new List<Landmark>();
        //    var mockRepo = new Mock<IDeletableEntityRepository<Landmark>>();
        //    mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
        //    mockRepo.Setup(x => x.AddAsync(It.IsAny<Landmark>()));
        //    mockRepo.Setup(x => x.Delete(It.IsAny<Landmark>()))
        //          .Callback(
        //          (Landmark pet) => list.Remove(pet));
        //    var service = new LandmarksService(mockRepo.Object);

        //    var user = new ApplicationUser
        //    {
        //        UserName = "user1",
        //        Email = "dada@abv.bg",
        //    };
        //    var landmark = new Landmark
        //    {
        //        Name = "landmark",
        //        CategoryId = 1,
        //        RegionId = 1,
        //        TownId = 1,
        //        MountainId = 1,
        //        Description = "description ,description description description description description description description description description description description description description description description description description description description description description description description description description description descriptiondescription description description description description description description description description description description description description description description description description  ",
        //        Latitude = 23.235,
        //        Longitute = 43.523,
        //        Websate = "www.abvg.bg",
        //        Address = "Address 1",
        //        PhoneNumber = "0888888888",
        //        WorkTime = "8:00-20:00",
        //        DayOff = "none",
        //        EntranceFee = 5,
        //        Difficulty = 4,
        //        UserId = user.Id,
        //    };

        //    list.Add(landmark);

        //    Assert.Equal(landmark, service.GetById<Landmark>(landmark.Id));
        //}
    }
}
