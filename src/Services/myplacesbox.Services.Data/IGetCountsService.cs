namespace MyPlacesBox.Services.Data
{
    using MyPlacesBox.Services.Data.Models;
    using MyPlacesBox.Web.ViewModels.Home;

    // 1. Use the view Model
    // 2. Create DTO -> view model
    // 3. Tupels
    public interface IGetCountsService
    {
        CountsDto GetCounts();
    }
}
