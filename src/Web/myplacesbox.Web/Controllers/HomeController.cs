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
        // private readonly IMapper mapper;

        public HomeController(IGetCountsService countsService)
        {
            this.countsService = countsService;
            // this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var countsDto = this.countsService.GetCounts();
            //// var viewModel = this.mapper.Map<IndexViewModel>(counts);
            var viewModel = new IndexViewModel
            {
                LandmarksCount = countsDto.LandmarksCount,
                HikesCount = countsDto.HikesCount,
                CategoriesCount = countsDto.CategoriesCount,
                RegionsCount = countsDto.RegionsCount,
                TownsCount = countsDto.TownsCount,
                MountainsCount = countsDto.MountainsCount,
                ImagesCount = countsDto.ImagesCount,
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
