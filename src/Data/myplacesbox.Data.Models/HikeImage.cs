namespace MyPlacesBox.Data.Models
{
    using System;

    using MyPlacesBox.Data.Common.Models;

    public class HikeImage : BaseDeletableModel<string>
    {
        public HikeImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int HikeId { get; set; }

        public virtual Hike Hike { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
