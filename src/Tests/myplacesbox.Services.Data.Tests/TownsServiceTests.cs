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

    public class TownsServiceTests
    {
        [Fact]
        public void AddTown()
        {
            var list = new List<Town>();
            var mockRepo = new Mock<IDeletableEntityRepository<Town>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Town>()));
            var service = new TownsService(mockRepo.Object);
            var city = new Town
            {
                Id = 1,
                Name = "Sandanski",
            };
            list.Add(city);
            var expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1", "Sandanski"),
            };
            Assert.Equal(expected, service.GetAllAsKeyValuePairs());
        }
    }
}
