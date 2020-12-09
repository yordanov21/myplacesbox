using MyPlacesBox.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Data.Models
{
    public class LandmarkImage : BaseModel<string>
    {
        public LandmarkImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
