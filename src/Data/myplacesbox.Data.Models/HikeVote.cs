namespace MyPlacesBox.Data.Models
{
    using MyPlacesBox.Data.Common.Models;

    public class HikeVote : BaseModel<int>
    {
        public int HikeId { get; set; }

        public virtual Hike Hike { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
