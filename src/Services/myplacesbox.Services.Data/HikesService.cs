using MyPlacesBox.Data.Common.Repositories;
using MyPlacesBox.Data.Models;
using MyPlacesBox.Web.ViewModels.Hikes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public class HikesService : IHikesService
    {
        private readonly IDeletableEntityRepository<Hike> hikesRepository;

        public HikesService(IDeletableEntityRepository<Hike> hikesRepository)
        {
            this.hikesRepository = hikesRepository;
        }

        public void Create(CreateHikeInputModel input)
        {
            // impement  Hike
            // TODO: NOT IMPLENET JET
        }
    }
}
