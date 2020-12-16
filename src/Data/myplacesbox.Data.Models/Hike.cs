namespace MyPlacesBox.Data.Models
{
    using System;
    using System.Collections.Generic;

    using MyPlacesBox.Data.Common.Models;

    public class Hike : BaseDeletableModel<int>
    {
        public Hike()
        {
            this.HikeImages = new HashSet<HikeImage>();
            this.HikeVotes = new HashSet<HikeVote>();
        }

        public string Name { get; set; }

        public double Length { get; set; }

        public int Denivelation { get; set; }

        public TimeSpan Duration { get; set; }

        public int? Marking { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int HikeStartPointId { get; set; }

        public virtual HikeStartPoint HikeStartPoint { get; set; }

        public int HikeEndPointId { get; set; }

        public virtual HikeEndPoint HikeEndPoint { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int MountainId { get; set; }

        public virtual Mountain Mountain { get; set; }

        public virtual ICollection<HikeImage> HikeImages { get; set; }

        public virtual ICollection<HikeVote> HikeVotes { get; set; }
    }
}
