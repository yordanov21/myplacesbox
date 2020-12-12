namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System.Linq;

    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class LandmarkInListInputModel : IMapFrom<Landmark>, IHaveCustomMappings
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
            configuration.CreateMap<Landmark, LandmarkInListInputModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                    x.LandmarkImages.FirstOrDefault().RemoteImageUrl != null ?
                    x.LandmarkImages.FirstOrDefault().RemoteImageUrl :
                    "/images/landmarks/" + x.LandmarkImages.FirstOrDefault().Id + "." + x.LandmarkImages.FirstOrDefault().Extension));
        }
    }
}
