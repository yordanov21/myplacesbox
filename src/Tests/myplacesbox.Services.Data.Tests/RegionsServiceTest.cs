namespace MyPlacesBox.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using Xunit;

    public class RegionsServiceTest
    {
        [Fact]
        public void AddingRegion()
        {
            var list = new List<Region>();
            var mockRepo = new Mock<IDeletableEntityRepository<Region>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Region>()));
            var service = new RegionsService(mockRepo.Object);
            var region = new Region
            {
                Id = 1,
                Name = "Sofia",
            };
            list.Add(region);
            var expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1", "Sofia"),
            };
            Assert.Equal(expected, service.GetAllAsKeyValuePairs());
        }
    }
}
