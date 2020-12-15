namespace MyPlacesBox.Web.ViewModels.HikeEndPoints
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateHikeEndPointInputModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        // Level High
        [Range(0, 8848)]
        public int Altitude { get; set; }

        // Global coordinates
        public double? Latitude { get; set; }

        public double? Longitute { get; set; }
    }
}
