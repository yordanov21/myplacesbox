namespace MyPlacesBox.Web.ViewModels.Votes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PostLandmarkVoteInputModel
    {
        public int LandmarkId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
