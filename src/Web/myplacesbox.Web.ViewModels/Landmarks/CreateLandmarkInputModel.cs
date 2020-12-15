namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class CreateLandmarkInputModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        // Global cordinates
        public double? Latitude { get; set; }

        public double? Longitute { get; set; }

        [MinLength(5)]
        public string Websate { get; set; }

        [MinLength(5)]
        public string Address { get; set; }

        [MinLength(7)]
        public string PhoneNumber { get; set; }

        [MinLength(4)]
        public string WorkTime { get; set; }

        [MinLength(4)]
        public string DayOff { get; set; }

        public double EntranceFee { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Range(1, 6)]
        public int Stars { get; set; }

        [Range(1, 6)]
        public int Difficulty { get; set; }

        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public int TownId { get; set; }

        public int MountainId { get; set; }

        public IEnumerable<IFormFile> LandmarkImages { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RegionsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TownsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> MountainsItems { get; set; }
    }
}
