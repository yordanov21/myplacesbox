namespace MyPlacesBox.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels.Landmarks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LandmarksController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IRegionsService regionsService;
        private readonly ITownsService townsService;
        private readonly IMountainsService mountainsService;
        private readonly ILandmarksService landmarksService;

        public LandmarksController(
            ICategoriesService categoriesService,
            IRegionsService regionsService,
            ITownsService townsService,
            IMountainsService mountainsService,
            ILandmarksService landmarksService
            )
        {
            this.categoriesService = categoriesService;
            this.regionsService = regionsService;
            this.townsService = townsService;
            this.mountainsService = mountainsService;
            this.landmarksService = landmarksService;
        }

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

            await this.landmarksService.CreateAsync(input);
            return this.Redirect("/");
        }
    }
}
