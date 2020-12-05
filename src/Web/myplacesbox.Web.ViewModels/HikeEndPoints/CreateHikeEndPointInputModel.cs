﻿using System;
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
        [Range(0, 8881)]
        public int StatrtPointAltitude { get; set; }

        // Global coordinates
        public double? StartPointLatitude { get; set; }

        public double? StartPointLongitute { get; set; }

    }
}
