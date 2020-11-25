﻿namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;
    using System;
    using System.Collections.Generic;

    public class Town : BaseDeletableModel<int>
    {
        public Town()
        {
            this.Landmarks = new HashSet<Landmark>();
            this.Hikes = new HashSet<Hike>();
        }

        public string Name { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }

        public ICollection<Hike> Hikes { get; set; }
    }
}