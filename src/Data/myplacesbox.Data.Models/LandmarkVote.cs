namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;

    public class LandmarkVote : BaseModel<int>
    {
        public int LandmarkId { get; set; }

        public virtual Landmark Landmark { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
