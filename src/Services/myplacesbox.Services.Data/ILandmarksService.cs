namespace MyPlacesBox.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Landmarks;

    public interface ILandmarksService : IAllPlacesService
    {
        // with input model
        Task CreateAsync(CreateLandmarkInputModel input, string userId, string imagePath);

        Task UpdateAsync(int id, EditLandmarkInputModel input);
    }
}
