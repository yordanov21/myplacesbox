namespace MyPlacesBox.Data.Models
{
    using System;

    using MyPlacesBox.Data.Common.Models;

    public class Landmark : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        // google link or cordinate
        public string Location { get; set; }

        public string Address { get; set; }

        public string Websate { get; set; }

        public string PhoneNumber { get; set; }

        public string WorkTime { get; set; }

        public string DayOff { get; set; }

        public double EntranceFee { get; set; }

        public string Description { get; set; }

        // 1-6
        public int Stars { get; set; }

        // enum maybe
        public int Difficulty { get; set; }

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
