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
    using MyPlacesBox.Services.Data.Models;
    using Xunit;

    public class GetCoutServiseTest
    {
        [Fact]
        public void GetCountTest()
        {
            var listHike = new List<Hike>();
            var listLandmark = new List<Landmark>();
            var listHikeImg = new List<HikeImage>();
            var listLandmarkImg = new List<LandmarkImage>();
            var listCategoty = new List<Category>();
            var listRegion = new List<Region>();
            var listTown = new List<Town>();
            var listMountain = new List<Mountain>();
           
      
            var mockRepoHikeImg = new Mock<IDeletableEntityRepository<HikeImage>>();
            var mockRepoLandmarkImg = new Mock<IRepository<LandmarkImage>>();
            var mockRepoCategoty = new Mock<IDeletableEntityRepository<Category>>();
            var mockRepoRegion = new Mock<IDeletableEntityRepository<Region>>();
            var mockRepoTown = new Mock<IDeletableEntityRepository<Town>>();
            var mockRepoMountain = new Mock<IDeletableEntityRepository<Mountain>>();

            var mockRepoHike = new Mock<IDeletableEntityRepository<Hike>>();
            mockRepoHike.Setup(x => x.All()).Returns(listHike.AsQueryable());
            mockRepoHike.Setup(x => x.AddAsync(It.IsAny<Hike>()));

            var mockRepoLandmark = new Mock<IDeletableEntityRepository<Landmark>>();
            mockRepoLandmark.Setup(x => x.All()).Returns(listLandmark.AsQueryable());
            mockRepoLandmark.Setup(x => x.AddAsync(It.IsAny<Landmark>()));

            mockRepoHikeImg.Setup(x => x.All()).Returns(listHikeImg.AsQueryable());
            mockRepoHikeImg.Setup(x => x.AddAsync(It.IsAny<HikeImage>()));

            mockRepoLandmarkImg.Setup(x => x.All()).Returns(listLandmarkImg.AsQueryable());
            mockRepoLandmarkImg.Setup(x => x.AddAsync(It.IsAny<LandmarkImage>()));

            mockRepoCategoty.Setup(x => x.All()).Returns(listCategoty.AsQueryable());
            mockRepoCategoty.Setup(x => x.AddAsync(It.IsAny<Category>()));

            mockRepoRegion.Setup(x => x.All()).Returns(listRegion.AsQueryable());
            mockRepoRegion.Setup(x => x.AddAsync(It.IsAny<Region>()));

            mockRepoTown.Setup(x => x.All()).Returns(listTown.AsQueryable());
            mockRepoTown.Setup(x => x.AddAsync(It.IsAny<Town>()));

            mockRepoMountain.Setup(x => x.All()).Returns(listMountain.AsQueryable());
            mockRepoMountain.Setup(x => x.AddAsync(It.IsAny<Mountain>()));

  
            var servise = new GetCountsService(
                mockRepoLandmark.Object,
                mockRepoHike.Object,
                mockRepoCategoty.Object,
                mockRepoRegion.Object,
                mockRepoTown.Object,
                mockRepoMountain.Object,
                mockRepoLandmarkImg.Object,
                mockRepoHikeImg.Object);

            //// var hikeService = new HikesService(mockRepoHike.Object);
            // var landmarkService = new LandmarksService(mockRepoLandmark.Object);
            // var categoryServise = new CategoriesService(mockRepoCategoty.Object);
            // var regionServise = new RegionsService(mockRepoRegion.Object);
            // var townServise = new TownsService(mockRepoTown.Object);
            // var mountainServise = new MountainsService(mockRepoMountain.Object);

            var expectedDto = new CountsDto();
             var actual = servise.GetCounts();
           
            Assert.Equal(0, actual.LandmarksCount);
            Assert.Equal(0, actual.HikesCount);
            Assert.Equal(0, actual.LandmarkImagesCount);
            Assert.Equal(0, actual.HikeImagesCount);
            Assert.Equal(0, actual.CategoriesCount);
            Assert.Equal(0, actual.RegionsCount);
            Assert.Equal(0, actual.TownsCount);
            Assert.Equal(0, actual.MountainsCount);
        }
    }
}
//public CountsDto GetCounts()
//{
//    var data = new CountsDto
//    {
//        LandmarksCount = this.landmarksRepository.All().Count(),
//        LandmarkImagesCount = this.landmarkImagesRepository.All().Count(),
//        HikesCount = this.hikesRepository.All().Count(),
//        HikeImagesCount = this.hikeImagesRepository.All().Count(),
//        CategoriesCount = this.categoriesRepository.All().Count(),
//        RegionsCount = this.regionsRepository.All().Count(),
//        TownsCount = this.townsRepository.All().Count(),
//        MountainsCount = this.mountainsRepository.All().Count(),
//    };

//    return data;
//}