﻿namespace MyPlacesBox.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels;
    using MyPlacesBox.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IGetCountsService countsService;
        private readonly ILandmarksService landmarksService;
        private readonly IHikesService hikesService;

        public HomeController(
            IGetCountsService countsService,
            ILandmarksService landmarksService,
            IHikesService hikesService)
        {
            this.countsService = countsService;
            this.landmarksService = landmarksService;
            this.hikesService = hikesService;
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
                RandomHikes = this.hikesService.GetRandom<IndexPageHikesViewModel>(6),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult AboutUs()
        {
            return this.View();
        }

        public IActionResult Team()
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
