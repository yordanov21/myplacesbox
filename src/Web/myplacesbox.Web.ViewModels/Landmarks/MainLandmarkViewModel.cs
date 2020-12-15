namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MainLandmarkViewModel
    {
        public SingleLandmarkViewModel SingleLandmarkView { get; set; }

        public LandmarksListInputModel LandmarksListInput { get; set; }
    }
}
