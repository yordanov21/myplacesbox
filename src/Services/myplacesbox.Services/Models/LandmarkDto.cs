using MyPlacesBox.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Services.Models
{
    public class LandmarkDto
    {
        public LandmarkDto()
        {
            this.Images = new List<Image>();
        }
        public string Name { get; set; }

        // Global cordinates (location)
        public double? Latitude { get; set; }

        public double? Longitute { get; set; }

        public string Websate { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string WorkTime { get; set; }

        public string DayOff { get; set; }

        public double? EntranceFee { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public int Difficulty { get; set; }

        public string CategoryName { get; set; }

        public string RegionName { get; set; }

        public string TownName { get; set; }

        public string MountainName { get; set; }


        public virtual ICollection<Image> Images { get; set; }
    }
}
