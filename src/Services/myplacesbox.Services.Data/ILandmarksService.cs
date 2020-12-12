namespace MyPlacesBox.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Landmarks;

    public interface ILandmarksService
    {
        // with input model
        Task CreateAsync(CreateLandmarkInputModel input, string userId, string imagePath);

        IEnumerable<T> GetAll<T>(int page, int itemsPage = 10);

        IEnumerable<T> GetRandom<T>(int count);

        int GetCount();

        T GetById<T>(int id);

        Task UpdateAsync(int id, EditLandmarkInputModel input);
    }
}
