namespace MyPlacesBox.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Services.Data;
    using MyPlacesBox.Web.ViewModels.Hikes;

    public class HikesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IHikesService hikesService;

        public HikesController(
            ICategoriesService categoriesService,
            IHikesService hikesService)
        {
            this.categoriesService = categoriesService;
            this.hikesService = hikesService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateHikeInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHikeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            await this.hikesService.CreateAsync(input);
            return this.Redirect("/");
        }
    }
}
