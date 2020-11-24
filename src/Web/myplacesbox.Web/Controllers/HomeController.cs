namespace MyPlacesBox.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Data;
    using MyPlacesBox.Web.ViewModels;
    using MyPlacesBox.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                LandmarksCount = this.db.Landmarks.Count(),
                HikesCount = this.db.Hikes.Count(),
                CategoriesCount = this.db.Categories.Count(),
                RegionsCount = this.db.Regions.Count(),
                TownsCount = this.db.Towns.Count(),
                MountainsCount = this.db.Mountains.Count(),
                ImagesCount = this.db.Images.Count(),
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
