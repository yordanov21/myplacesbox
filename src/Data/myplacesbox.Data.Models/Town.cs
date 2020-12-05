namespace MyPlacesBox.Data.Models
{
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public Town()
        {
            this.Landmarks = new HashSet<Landmark>();
            this.Hikes = new HashSet<Hike>();
        }

        public string Name { get; set; }

        public bool IsTown { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}
