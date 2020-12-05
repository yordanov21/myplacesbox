namespace MyPlacesBox.Data.Models
{
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class HikeStartPoint : BaseDeletableModel<int>
    {
        public HikeStartPoint()
        {
            this.Hikes = new HashSet<Hike>();
        }

        public string Name { get; set; }

        // Level High
        public int Altitude { get; set; }

        // Global cordinates (location)
        public double? Latitude { get; set; }

        public double? Longitute { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}
