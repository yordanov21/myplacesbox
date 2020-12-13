namespace MyPlacesBox.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Common;
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
        private readonly IWebHostEnvironment hostEnvironment;

        public HikesController(
            ICategoriesService categoriesService,
            IRegionsService regionsService,
            IMountainsService mountainsService,
            IHikesService hikesService,
            IHikeStartPointsService hikeStartPointsService,
            IHikeEndPointsService hikeEndPointsService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostEnvironment)
        {
            this.categoriesService = categoriesService;
            this.regionsService = regionsService;
            this.mountainsService = mountainsService;
            this.hikesService = hikesService;
            this.hikeStartPointsService = hikeStartPointsService;
            this.hikeEndPointsService = hikeEndPointsService;
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var inputModel = this.hikesService.GetById<EditHikekInputModel>(id);
            inputModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            inputModel.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
            inputModel.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, EditHikekInputModel input)
        {
            ;

            //if (!this.ModelState.IsValid)
            //{
            //    input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            //    input.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
            //    input.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();
            //    return this.View(input);
            //}

            await this.hikesService.UpdateAsync(id, input);
            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateHikeInputModel
            {
                CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs(),
                RegionsItems = this.regionsService.GetAllAsKeyValuePairs(),
                MountainsItems = this.mountainsService.GetAllAsKeyValuePairs(),
                // HikeStartPoint = new HikeStartPointsService,
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
            try
            {
                await this.hikesService.CreateAsync(input, user.Id, $"{this.hostEnvironment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);

                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                input.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
                input.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            return this.Redirect("/");
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 10;
            var viewModel = new HikesListInputModel
            {
                PageNumber = id,
                ColectionCount = this.hikesService.GetCount(),
                Hikes = this.hikesService.GetAll<HikeInListInputModel>(id, ItemsPerPage),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {

            var hike = this.hikesService.GetById<SingleHikeViewModel>(id);

            var hikes = new HikesListInputModel
            {
                PageNumber = id,
                ColectionCount = this.hikesService.GetCount(),
                Hikes = this.hikesService.GetAll<HikeInListInputModel>(1, this.hikesService.GetCount()),
                ItemsPerPage = 10,
            };

            MainHikeViewModel mainLandmarkView = new MainHikeViewModel
            {
                SingleHikeView = hike,
                HikesListInput = hikes,
            };

            return this.View(mainLandmarkView);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.hikesService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
