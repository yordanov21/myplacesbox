namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using System.Linq;

    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class HikeInListInputModel : IMapFrom<Hike>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public int Stars { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hike, HikeInListInputModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                    x.HikeImages.FirstOrDefault().RemoteImageUrl != null ?
                    x.HikeImages.FirstOrDefault().RemoteImageUrl :
                    "/images/landmarks/" + x.HikeImages.FirstOrDefault().Id + "." + x.HikeImages.FirstOrDefault().Extension));
        }
    }
}
