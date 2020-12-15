namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System.Collections.Generic;

    public class LandmarksListInputModel : PagingViewModel
    {
        public IEnumerable<LandmarkInListInputModel> Landmarks { get; set; }
    }
}
