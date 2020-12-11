using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyPlacesBox.Web.ViewModels.Votes
{
    public class PostLandmarkVoteInputModel
    {
        public int LandmarkId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
