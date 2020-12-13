namespace MyPlacesBox.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class IndexPageHikesViewModel : IMapFrom<Hike>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hike, IndexPageHikesViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.HikeImages.FirstOrDefault().RemoteImageUrl != null ?
                        x.HikeImages.FirstOrDefault().RemoteImageUrl :
                        "/images/hikes/" + x.HikeImages.FirstOrDefault().Id + "." + x.HikeImages.FirstOrDefault().Extension));
        }

    }
}
