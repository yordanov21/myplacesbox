namespace MyPlacesBox.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels.Hikes;

    public class HikesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IRegionsService regionsService;
        private readonly IMountainsService mountainsService;
        private readonly IHikesService hikesService;
        private readonly IHikeStartPointsService hikeStartPointsService;
        private readonly IHikeEndPointsService hikeEndPointsService;

        public HikesController(
            ICategoriesService categoriesService,
            IRegionsService regionsService,
            IMountainsService mountainsService,
            IHikesService hikesService,
            IHikeStartPointsService hikeStartPointsService,
            IHikeEndPointsService hikeEndPointsService
           )
        {
            this.categoriesService = categoriesService;
            this.regionsService = regionsService;
            this.mountainsService = mountainsService;
            this.hikesService = hikesService;
            this.hikeStartPointsService = hikeStartPointsService;
            this.hikeEndPointsService = hikeEndPointsService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateHikeInputModel
            {
                CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs(),
                RegionsItems = this.regionsService.GetAllAsKeyValuePairs(),
                MountainsItems = this.mountainsService.GetAllAsKeyValuePairs(),
                //HikeStartPoint = new HikeStartPointsService,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHikeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                input.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
                input.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            await this.hikesService.CreateAsync(input);
            return this.Redirect("/");
        }
    }
}
