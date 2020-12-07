using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    public class LandmarksListInputModel
    {
        public IEnumerable<LandmarkInListInputModel> Landmarks { get; set; }

        public int PageNumber { get; set; }
    }
}
