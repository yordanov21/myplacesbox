using MyPlacesBox.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Data.Models
{
    public class HikeImage : BaseDeletableModel<int>
    {
        //public HikeImage()
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}

        //public int HikeId { get; set; }

        //public int HikeId { get; set; }

        public virtual Hike Hike { get; set; }

        public string UrlPath { get; set; }

        //// The contents of the image is in the file system

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
