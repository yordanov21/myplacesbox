namespace MyPlacesBox.Web.ViewModels.Votes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PostHikeVoteInputModel
    {
        public int HikeId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
