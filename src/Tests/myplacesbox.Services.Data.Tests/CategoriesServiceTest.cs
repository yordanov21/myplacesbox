namespace MyPlacesBox.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using Xunit;

    public class CategoriesServiceTest
    {
        // Hike tests
        [Fact]
        public void GetAllHikeCategotiesAsKeyValuePairsTest()
        {
            var categoriesList = new List<Category>();
            var mockRepo = new Mock<IDeletableEntityRepository<Category>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Category>()));
            var service = new CategoriesService(mockRepo.Object);
            Category categoryInput = new Category
            {
                Id = 1,
                Name = "hike-Category",
                Type = "Hike",
            };
            Category categoryInput2 = new Category
            {
                Id = 2,
                Name = "landmark-Category",
                Type = "Landmark",
            };
            categoriesList.Add(categoryInput);
            categoriesList.Add(categoryInput2);

            var expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1", "hike-Category"),
            };

            var actual = service.GetAllHikeCategotiesAsKeyValuePairs();
            Assert.Equal(expected, actual);
        }

        // Landmark tests
        [Fact]
        public void GetAllHLandmarkCategotiesAsKeyValuePairsTest()
        {
            var categoriesList = new List<Category>();
            var mockRepo = new Mock<IDeletableEntityRepository<Category>>();
            mockRepo.Setup(x => x.AllAsNoTracking()).Returns(categoriesList.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Category>()));
            var service = new CategoriesService(mockRepo.Object);
            Category categoryInput = new Category
            {
                Id = 1,
                Name = "hike-Category",
                Type = "Hike",
            };
            Category categoryInput2 = new Category
            {
                Id = 2,
                Name = "landmark-Category",
                Type = "Landmark",
            };
            categoriesList.Add(categoryInput);
            categoriesList.Add(categoryInput2);

            var expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("2", "landmark-Category"),
            };

            var actual = service.GetAllLandmarkCategotiesAsKeyValuePairs();
            Assert.Equal(expected, actual);
        }
    }
}
