using MyPlacesBox.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Data.Models
{
    public class LandmarkImage : BaseDeletableModel<int>
    {
        //public LandmarkImage()
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}

        //public int LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; }

        public string UrlPath { get; set; }

        //// The contents of the image is in the file system

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
