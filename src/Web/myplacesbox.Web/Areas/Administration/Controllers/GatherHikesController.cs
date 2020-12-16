namespace MyPlacesBox.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services;

    public class GatherHikesController : BaseController
    {
        private readonly IHikeScraperService hikesScraperService;

        public GatherHikesController(IHikeScraperService hikesScraperService)
        {
            this.hikesScraperService = hikesScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.hikesScraperService.PopulateDbWithHikesAsync();
            return this.Redirect("/");
        }
    }
}
