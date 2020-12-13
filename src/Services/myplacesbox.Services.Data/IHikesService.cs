namespace MyPlacesBox.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MyPlacesBox.Web.ViewModels.Hikes;

    public interface IHikesService : IAllPlacesService
    {
        // with input model
        Task CreateAsync(CreateHikeInputModel input, string userId, string imagePath);

        Task UpdateAsync(int id, EditHikekInputModel input);
    }
}
