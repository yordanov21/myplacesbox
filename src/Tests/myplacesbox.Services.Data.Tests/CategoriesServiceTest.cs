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
            mockRepo.Setup(x => x.All()).Returns(categoriesList.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Category>())).Callback(
                (Category category) => categoriesList.Add(category));

            Category categoryInput = new Category
            {
                Id = 1,
                Name = "Category",
                Type = "Hike",
            };
            Category categoryInput2 = new Category
            {
                Id = 2,
                Name = "Category2",
                Type = "Hike",
            };
            categoriesList.Add(categoryInput);
            categoriesList.Add(categoryInput2);

            var service = new CategoriesService(mockRepo.Object);
            service.GetAllHikeCategotiesAsKeyValuePairs();

            Assert.Equal(2, categoriesList.Count);
            Assert.Equal(1, categoriesList[0].Id);
            Assert.Equal("Category", categoriesList[0].Name);
        }

        // landmark tests
        [Fact]
        public void GetAllLandmarkCategotiesAsKeyValuePairsTest()
        {
            var categoriesList = new List<Category>();
            Category categoryInput = new Category
            {
                Id = 1,
                Name = "Category",
            };
            Category categoryInput2 = new Category
            {
                Id = 2,
                Name = "Category2",
            };
            categoriesList.Add(categoryInput);
            categoriesList.Add(categoryInput2);

            var mockRepo = new Mock<IDeletableEntityRepository<Category>>();
            mockRepo.Setup(x => x.All()).Returns(categoriesList.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Category>())).Callback(
                (Category category) => categoriesList.Add(category));

            var service = new CategoriesService(mockRepo.Object);
            var result = service.GetAllLandmarkCategotiesAsKeyValuePairs().ToList().Count;

            Assert.Equal(0, result);
        }
    }
}
