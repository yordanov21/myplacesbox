namespace MyPlacesBox.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Common;
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

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var inputModel = this.landmarksService.GetById<EditLandmarkInputModel>(id);
            inputModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            inputModel.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
            inputModel.TownsItems = this.townsService.GetAllAsKeyValuePairs();
            inputModel.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, EditLandmarkInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                input.RegionsItems = this.regionsService.GetAllAsKeyValuePairs();
                input.TownsItems = this.townsService.GetAllAsKeyValuePairs();
                input.MountainsItems = this.mountainsService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            await this.landmarksService.UpdateAsync(id, input);
            return this.RedirectToAction(nameof(this.ById), new { id });
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
                ColectionCount = this.landmarksService.GetCount(),
                Landmarks = this.landmarksService.GetAll<LandmarkInListInputModel>(id, ItemsPerPage),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {

            var landmark = this.landmarksService.GetById<SingleLandmarkViewModel>(id);

            var landmarks = new LandmarksListInputModel
            {
                PageNumber = id,
                ColectionCount = this.landmarksService.GetCount(),
                Landmarks = this.landmarksService.GetAll<LandmarkInListInputModel>(1, this.landmarksService.GetCount()),
                ItemsPerPage = 10,
            };

            MainLandmarkViewModel mainLandmarkView = new MainLandmarkViewModel
            {
                SingleLandmarkView = landmark,
                LandmarksListInput = landmarks,
            };

            return this.View(mainLandmarkView);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.landmarksService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }

        //[HttpPost]
        //public async Task<IActionResult> SendToEmail(int id)
        //{
        //    var recipe = this.landmarksService.GetById<LandmarkInListViewModel>(id);
        //    var html = new StringBuilder();
        //    html.AppendLine($"<h1>{recipe.Name}</h1>");
        //    html.AppendLine($"<h3>{recipe.CategoryName}</h3>");
        //    html.AppendLine($"<img src=\"{recipe.ImageUrl}\" />");
        //    await this.emailSender.SendEmailAsync("recepti@recepti.com", "MoiteRecepti", "gerig14198@questza.com", recipe.Name, html.ToString());
        //    return this.RedirectToAction(nameof(this.ById), new { id });
        //}
    }
}
