namespace MyPlacesBox.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Moq;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using Xunit;

    public class MountainsServiceTest
    {
        [Fact]
        public void AddingMountain()
        {
            var list = new List<Mountain>();
            var mockRepo = new Mock<IDeletableEntityRepository<Mountain>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Mountain>()));
            var service = new MountainsService(mockRepo.Object);
            var mountain = new Mountain
            {
                Id = 1,
                Name = "Rila",
            };
            list.Add(mountain);
            var expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1", "Rila"),
            };
            Assert.Equal(expected, service.GetAllAsKeyValuePairs());
        }
    }
}
