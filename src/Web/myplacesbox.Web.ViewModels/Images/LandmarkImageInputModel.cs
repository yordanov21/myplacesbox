namespace MyPlacesBox.Web.ViewModels.Images
{
    using System.ComponentModel.DataAnnotations;

    public class LandmarkImageInputModel
    {
            [Required]
            [MinLength(30)]
            public string UrlPath { get; set; }
    }
}
