using Moq;
using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using MyPlacesBox.Web.ViewModels.Landmarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyPlacesBox.Services.Data.Tests
{
    public class LandmarksServiceTest
    {
        [Fact]
        public async Task GetCountShouldBeCorrect()
        {
            var list = new List<Landmark>();
            var mockRepo = new Mock<IDeletableEntityRepository<Landmark>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Landmark>())).Callback(
                (Landmark vote) => list.Add(vote));
            var service = new LandmarksService(mockRepo.Object);

            var landmark = new CreateLandmarkInputModel();
            var user = new ApplicationUser();
            await service.CreateAsync(landmark, user.Id, "imagepath");

            mockRepo.Verify(x => x.AddAsync(It.IsAny<Landmark>()), Times.Exactly(1));

            Assert.Equal(1, service.GetCount());        
        }
        //public int GetCount()
        //{
        //    return this.landmarksRepository.All().Count();
        //}
    }
}
