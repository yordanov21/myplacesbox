namespace MyPlacesBox.Data.Models
{
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class HikeEndPoint : BaseDeletableModel<int>
    {
        public HikeEndPoint()
        {
            this.Hikes = new HashSet<Hike>();
        }

        public string Name { get; set; }

        // Level High
        public int StatrtPointAltitude { get; set; }

        // Global cordinates (location)
        public double? StartPointLatitude { get; set; }

        public double? StartPointLongitute { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}
