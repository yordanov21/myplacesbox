namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;
    using System;
    using System.Collections.Generic;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Extension { get; set; }

        //The contents of the image is in the file system
    }
}
