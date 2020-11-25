using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyPlacesBox.Web.ViewModels.Hikes
{
    public class CreateHikeInputModel
    {
        public CreateHikeInputModel()
        {
            
        }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string StartingPoint { get; set; }

        [Required]
        [MinLength(5)]
        public string EndPoint { get; set; }


        public string Location { get; set; }
  
        public int Denivelation { get; set; }

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

        [Display(Name = "Category")]
        public int CategoryId { get; set; }


        public int RegionId { get; set; }


        public int TownId { get; set; }


        public int MountainId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
