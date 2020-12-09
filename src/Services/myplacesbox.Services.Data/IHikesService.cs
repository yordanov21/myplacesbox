namespace MyPlacesBox.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Hikes;

    public interface IHikesService
    {
        // with input model
        Task CreateAsync(CreateHikeInputModel input, string userId, string imagePath);

        IEnumerable<T> GetAll<T>(int page, int itemsPage = 10);

        int GetCount();
    }
}
