namespace MyPlacesBox.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexPageLandmarkViewModel> RandomLandmarks { get; set; }
        public IEnumerable<IndexPageHikesViewModel> RandomHikes { get; set; }

        public int LandmarksCount { get; set; }

        public int LandmarkImagesCount { get; set; }

        public int HikesCount { get; set; }

        public int HikeImagesCount { get; set; }

        public int CategoriesCount {get; set; }

        public int RegionsCount { get; set; }

        public int TownsCount { get; set; }

        public int MountainsCount { get; set; }
    }
}
