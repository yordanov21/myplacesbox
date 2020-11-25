namespace MyPlacesBox.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyPlacesBox.Web.ViewModels.Landmarks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LandmarksController : Controller
    {
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateLandmarkInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            // TODO: Create lanmark using service method
            // TODO: Redirect to landmark info page
            return this.Redirect("/");
        }
    }
}
