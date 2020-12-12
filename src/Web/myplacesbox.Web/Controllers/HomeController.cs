namespace MyPlacesBox.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Data;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels;
    using MyPlacesBox.Web.ViewModels.Home;

    // 1.with ApplicationDbContext
    // 1.with Repository
    public class HomeController : BaseController
    {
        private readonly IGetCountsService countsService;
        private readonly ILandmarksService landmarksService;

        public HomeController(
            IGetCountsService countsService,
            ILandmarksService landmarksService)
        {
            this.countsService = countsService;
            this.landmarksService = landmarksService;
        }

        public IActionResult Index()
        {
            var countsDto = this.countsService.GetCounts();
            var viewModel = new IndexViewModel
            {
                LandmarksCount = countsDto.LandmarksCount,
                LandmarkImagesCount = countsDto.LandmarkImagesCount,
                HikesCount = countsDto.HikesCount,
                HikeImagesCount = countsDto.HikeImagesCount,
                CategoriesCount = countsDto.CategoriesCount,
                RegionsCount = countsDto.RegionsCount,
                TownsCount = countsDto.TownsCount,
                MountainsCount = countsDto.MountainsCount,
                RandomLandmarks = this.landmarksService.GetRandom<IndexPageLandmarkViewModel>(6),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
