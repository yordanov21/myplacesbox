using MyPlacesBox.Web.ViewModels.Hikes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPlacesBox.Services.Data
{
    public interface IHikesService
    {
        // with input model
        Task CreateAsync(CreateHikeInputModel input, string userId);
    }
}
