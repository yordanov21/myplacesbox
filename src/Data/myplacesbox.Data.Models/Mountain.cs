namespace MyPlacesBox.Data.Models
{
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class Mountain : BaseDeletableModel<int>
    {
        public Mountain()
        {
            this.Landmarks = new HashSet<Landmark>();
            this.Hikes = new HashSet<Hike>();
        }

        public string Name { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}
