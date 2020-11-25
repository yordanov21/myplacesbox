using MyPlacesBox.Web.ViewModels.Hikes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public interface IHikesService
    {
        // with input model
        void Create(CreateHikeInputModel input);
    }
}
