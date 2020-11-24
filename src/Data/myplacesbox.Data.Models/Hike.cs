namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;
    using System;

    public class Hike : BaseDeletableModel<int>
    {
        public Hike()
        {

        }

        public string Name { get; set; }

        public string StartingPoint { get; set; }

        public string EndPoint { get; set; }

        public string Location { get; set; }

        public string Denivelation { get; set; }

        public string Length { get; set; }

        public TimeSpan Duration { get; set; }

        public int Marking { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

        public int? Stars { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }


    }
}
