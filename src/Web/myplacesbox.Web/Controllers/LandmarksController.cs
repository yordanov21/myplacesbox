namespace MyPlacesBox.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels.Landmarks;

    public class LandmarksController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IRegionsService regionsService;
        private readonly ITownsService townsService;
        private readonly IMountainsService mountainsService;
        private readonly ILandmarksService landmarksService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public LandmarksController(
            ICategoriesService categoriesService,
            IRegionsService regionsService,
            ITownsService townsService,
            IMountainsService mountainsService,
            ILandmarksService landmarksService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostEnvironment)
        {
            this.categoriesService = categoriesService;
            this.regionsService = regionsService;
            this.townsService = townsService;
            this.mountainsService = mountainsService;
            this.landmarksService = landmarksService;
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateLandmarkInputModel
            {
                CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs(),
                RegionsItems = this.regionsService.GetAllAsKeyValuePairs(),
                TownsItems = this.townsService.GetAllAsKeyValuePairs(),
                MountainsItems = this.mountainsService.GetAllAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateLandmarkInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.categoriesService.GetAllAsKeyValuePairs();
                this.regionsService.GetAllAsKeyValuePairs();
                this.townsService.GetAllAsKeyValuePairs();
                this.mountainsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            // Get userId by cookie
            //var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Get userId by UserManager
            var user = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.landmarksService.CreateAsync(input, user.Id, $"{this.hostEnvironment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);

                this.categoriesService.GetAllAsKeyValuePairs();
                this.regionsService.GetAllAsKeyValuePairs();
                this.townsService.GetAllAsKeyValuePairs();
                this.mountainsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }
           
            return this.Redirect("/");
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 10;
            var viewModel = new LandmarksListInputModel
            {
                PageNumber = id,
                LandmarksCount = this.landmarksService.GetCount(),
                Landmarks = this.landmarksService.GetAll<LandmarkInListInputModel>(id, ItemsPerPage),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }
    }
}
