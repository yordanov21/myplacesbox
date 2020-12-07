namespace MyPlacesBox.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Landmarks;

    public interface ILandmarksService
    {
        // with input model
        Task CreateAsync(CreateLandmarkInputModel input, string userId);

        IEnumerable<T> GetAll<T>(int page, int itemsPage = 10);
    }
}
