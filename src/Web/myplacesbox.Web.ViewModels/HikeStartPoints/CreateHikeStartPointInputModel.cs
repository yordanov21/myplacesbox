namespace MyPlacesBox.Web.ViewModels.HikeStartPoints
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateHikeStartPointInputModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        // Level High
        public int StatrtPointAltitude { get; set; }

        // Global coordinates
        public double? StartPointLatitude { get; set; }

        public double? StartPointLongitute { get; set; }

    }
}
