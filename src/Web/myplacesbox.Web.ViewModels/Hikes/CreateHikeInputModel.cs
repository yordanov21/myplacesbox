namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using MyPlacesBox.Web.ViewModels.HikeEndPoints;
    using MyPlacesBox.Web.ViewModels.HikeStartPoints;

    public class CreateHikeInputModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Range(0, 10000)]
        [Display(Name = "Length (in kilometers)")]
        public double Length { get; set; }

        [Range(0, 24 * 60 * 30)]
        [Display(Name = "Duration time (in minutes)")]
        public int Duration { get; set; }

        [Range(1, 6)]
        public int Marking { get; set; }

        [Range(1, 6)]
        public int Difficulty { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        public int HikeStartPointId { get; set; }

        public virtual CreateHikeStartPointInputModel HikeStartPoint { get; set; }

        public int HikeEndPointId { get; set; }

        public virtual CreateHikeEndPointInputModel HikeEndPoint { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public int MountainId { get; set; }

        public IEnumerable<IFormFile> HikeImages { get; set; }

        // TODO: Images for hikes not created.
        // public virtual IEnumerable<HikeImageInputModel> HikeImages { get; set; }
        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RegionsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> MountainsItems { get; set; }
    }
}
