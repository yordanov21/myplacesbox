namespace MyPlacesBox.Web.ViewModels.Home
{
    using System.Linq;

    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class IndexPageLandmarkViewModel : IMapFrom<Landmark>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Landmark, IndexPageLandmarkViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.LandmarkImages.FirstOrDefault().RemoteImageUrl != null ?
                        x.LandmarkImages.FirstOrDefault().RemoteImageUrl :
                        "/images/landmarks/" + x.LandmarkImages.FirstOrDefault().Id + "." + x.LandmarkImages.FirstOrDefault().Extension));
        }
    }
}
