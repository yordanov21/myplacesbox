namespace MyPlacesBox.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Data.Models;
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
        private readonly UserManager<ApplicationUser> userManager;

        public HikesController(
            ICategoriesService categoriesService,
            IRegionsService regionsService,
            IMountainsService mountainsService,
            IHikesService hikesService,
            IHikeStartPointsService hikeStartPointsService,
            IHikeEndPointsService hikeEndPointsService,
            UserManager<ApplicationUser> userManager)
        {
            this.categoriesService = categoriesService;
            this.regionsService = regionsService;
            this.mountainsService = mountainsService;
            this.hikesService = hikesService;
            this.hikeStartPointsService = hikeStartPointsService;
            this.hikeEndPointsService = hikeEndPointsService;
            this.userManager = userManager;
        }

        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Create(CreateHikeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                input.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
                input.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.hikesService.CreateAsync(input, user.Id);
            return this.Redirect("/");
        }
    }
}
