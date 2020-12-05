namespace MyPlacesBox.Data.Models
{
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class Region : BaseDeletableModel<int>
    {
        public Region()
        {
            this.Landmarks = new HashSet<Landmark>();
        }

        public string Name { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}
