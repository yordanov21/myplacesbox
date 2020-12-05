using MyPlacesBox.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Data.Models
{
    public class LandmarkImage : BaseDeletableModel<int>
    {
        public virtual Landmark Landmark { get; set; }

        public string UrlPath { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
