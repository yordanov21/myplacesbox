using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyPlacesBox.Web.ViewModels.HikeEndPoints
{
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
