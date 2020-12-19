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

    public class HikesServiceTest
    {
        [Fact]
        public async Task DeleteAsyncHike()
        {
            var list = new List<Hike>();
            var mockRepo = new Mock<IDeletableEntityRepository<Hike>>();
            var mockRepoStartPoint = new Mock<IDeletableEntityRepository<HikeStartPoint>>();
            var mockRepoStartEnd = new Mock<IDeletableEntityRepository<HikeEndPoint>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Hike>()));
            mockRepo.Setup(x => x.Delete(It.IsAny<Hike>()))
                  .Callback(
                  (Hike hike) => list.Remove(hike));

            var service = new HikesService(mockRepo.Object, mockRepoStartPoint.Object, mockRepoStartEnd.Object);

            var user = new ApplicationUser
            {
                UserName = "user1",
                Email = "dada@abv.bg",
            };
            var hike = new Hike
            {
                Name = "landmark",
                CategoryId = 1,
                RegionId = 1,
                MountainId = 1,
                Description = "description ,description description description description description description description description description description description description description description description description description description description description description description description description description description descriptiondescription description description description description description description description description description description description description description description description description  ",
                HikeStartPointId = 1,
                HikeEndPointId = 1,
                Marking = 2,
                Denivelation = 500,
                Duration = TimeSpan.FromMinutes(50),
                Difficulty = 4,
                UserId = user.Id,
            };

            list.Add(hike);

            await service.DeleteAsync(hike.Id);

            Assert.Equal(0, service.GetCount());
        }
    }
}
