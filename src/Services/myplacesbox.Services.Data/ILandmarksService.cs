namespace MyPlacesBox.Services.Data
{
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Landmarks;

    public interface ILandmarksService
    {
        // with input model
        Task CreateAsync(CreateLandmarkInputModel input);
    }
}
