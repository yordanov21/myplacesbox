using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    public class CreateLandmarkInputModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        // google link or cordinate
        public string Location { get; set; }

        [MinLength(5)]
        public string Websate { get; set; }

        [MinLength(7)]
        public string PhoneNumber { get; set; }

        public string WorkTime { get; set; }

        public string DayOff { get; set; }

        public double EntranceFee { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        [Range(1, 6)]
        public int Difficulty { get; set; }

        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public int TownId { get; set; }

        public int MountainId { get; set; }

    }
}
