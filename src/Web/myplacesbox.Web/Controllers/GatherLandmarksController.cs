namespace MyPlacesBox.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services;

    // TODO: Move in administarion area (only admin can access the controller)
    public class GatherLandmarksController : BaseController
    {
        private readonly ILandmarksScraperService landmarksScraperService;

        public GatherLandmarksController(ILandmarksScraperService landmarksScraperService)
        {
            this.landmarksScraperService = landmarksScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.landmarksScraperService.PopulateDbWithLandmarksAsync();
            return this.View();
        }
    }
}
