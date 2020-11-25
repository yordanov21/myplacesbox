using Microsoft.AspNetCore.Mvc;
using MyPlacesBox.Services.Data;
using MyPlacesBox.Web.ViewModels.Hikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPlacesBox.Web.Controllers
{
    public class HikesController : Controller
    {
        private readonly ICategoriesService categoriesService;

        public HikesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateHikeInputModel();
            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateHikeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            return this.Json(input);

            return this.Redirect("/");
        }
    }
}
