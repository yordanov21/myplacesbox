using Microsoft.AspNetCore.Mvc;
using MyPlacesBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MyPlacesBox.Web.Controllers
{
    public class GatherHikesController : BaseController
    {
        private readonly IHikeScraperService hikesScraperService;

        public GatherHikesController(IHikeScraperService hikesScraperService)
        {
            this.hikesScraperService = hikesScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            ;
            await this.hikesScraperService.PopulateDbWithHikesAsync();
            return this.View();
        }
    }
}