namespace MyPlacesBox.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MyPlacesBox.Data.Models;

    public class HikeDto
    {
        public HikeDto()
        {
            this.HikeImages = new HashSet<HikeImage>();
        }

        public string Name { get; set; }

        public double Length { get; set; }

        public int Denivelation { get; set; }

        public TimeSpan Duration { get; set; }

        public int? Marking { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public string HikeStartPointName { get; set; }

        public int HikeStartPointAltitude { get; set; }

        public double HikeStartPointLatitude { get; set; }

        public double HikeStartPointLongitude { get; set; }

        public string HikeEndPointName { get; set; }

        public int HikeEndPointAltitude { get; set; }

        public double HikeEndPointLatitude { get; set; }

        public double HikeEndPointLongitude { get; set; }

        public string CategoryName { get; set; }

        public string RegionName { get; set; }

        public string MountainName { get; set; }

        public virtual ICollection<HikeImage> HikeImages { get; set; }
    }
}
