namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;
    using System;
    using System.Collections.Generic;

    public class Town : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }
    }
}
