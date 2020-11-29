namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using MyPlacesBox.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateHikeInputModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

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

        [Range(1, 5)]
        public int Stars { get; set; }

        public int HikeStartPointId { get; set; }

        public virtual HikeStartPoint HikeStartPoint { get; set; }

        public int HikeEndPointId { get; set; }

        public virtual HikeEndPoint HikeEndPoint { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        //   public virtual ICollection<Image> Images { get; set; }


        //  public IEnumerable<IFormFile> Images { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
