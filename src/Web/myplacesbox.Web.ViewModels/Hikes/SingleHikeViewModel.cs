namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class SingleHikeViewModel : IMapFrom<Hike>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public double Length { get; set; }

        public int Denivelation { get; set; }

        public TimeSpan Duration { get; set; }

        public int? Marking { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

        public int Stars { get; set; }

        public string HikeStartPointName { get; set; }

        public virtual HikeStartPoint HikeStartPoint { get; set; }

        public string HikeEndPointName { get; set; }

        public virtual HikeEndPoint HikeEndPoint { get; set; }

        public string CategoryName { get; set; }

        public string RegionName { get; set; }

        public string MountainName { get; set; }

        public string ImageUrl { get; set; }

        public string UserUserName { get; set; }

        public double AverageVote { get; set; }

        public virtual ICollection<HikeImage> HikeImages { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hike, SingleHikeViewModel>()
               .ForMember(x => x.AverageVote, opt =>
                   opt.MapFrom(x => x.HikeVotes.Count() == 0 ? 0 : x.HikeVotes.Average(v => v.Value)))
               .ForMember(x => x.ImageUrl, opt =>
                   opt.MapFrom(x =>
                   x.HikeImages.FirstOrDefault().RemoteImageUrl != null ?
                   x.HikeImages.FirstOrDefault().RemoteImageUrl :
                   "/images/hikes/" + x.HikeImages.FirstOrDefault().Id + "." + x.HikeImages.FirstOrDefault().Extension));
        }
    }
}
