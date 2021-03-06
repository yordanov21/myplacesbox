﻿namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using MyPlacesBox.Data.Common.Repositories;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class SingleLandmarkViewModel : IMapFrom<Landmark>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public double? Latitude { get; set; }

        public double? Longitute { get; set; }

        public string Address { get; set; }

        public string Websate { get; set; }

        public string PhoneNumber { get; set; }

        public string WorkTime { get; set; }

        public string DayOff { get; set; }

        public double EntranceFee { get; set; }

        public string Description { get; set; }

        public int Difficulty { get; set; }

        public string CategoryName { get; set; }

        public string RegionName { get; set; }

        public string TownName { get; set; }

        public string MountainName { get; set; }

        public string ImageUrl { get; set; }

        public string UserUserName { get; set; }

        public double AverageVote { get; set; }

        public virtual ICollection<LandmarkImage> LandmarkImages { get; set; }

        public virtual IDeletableEntityRepository<Landmark> Landmarks { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Landmark, SingleLandmarkViewModel>()
                .ForMember(x => x.AverageVote, opt =>
                    opt.MapFrom(x => x.LandmarkVotes.Count() == 0 ? 0 : x.LandmarkVotes.Average(v => v.Value)))
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                    x.LandmarkImages.FirstOrDefault().RemoteImageUrl != null ?
                    x.LandmarkImages.FirstOrDefault().RemoteImageUrl :
                    "/images/landmarks/" + x.LandmarkImages.FirstOrDefault().Id + "." + x.LandmarkImages.FirstOrDefault().Extension));
        }
    }
}
