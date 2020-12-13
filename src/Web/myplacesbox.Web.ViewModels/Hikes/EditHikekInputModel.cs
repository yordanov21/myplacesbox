namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using AutoMapper;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;
    using MyPlacesBox.Web.ViewModels.HikeEndPoints;
    using MyPlacesBox.Web.ViewModels.HikeStartPoints;

    public class EditHikekInputModel : IMapFrom<Hike>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        [Range(0, 10000)]
        [Display(Name = "Length (in kilometers)")]
        public double Length { get; set; }

        [Range(0, 24 * 60 * 30)]
        [Display(Name = "Duration time (in minutes)")]
        public int Duration { get; set; }

        [Range(1, 6)]
        public int Marking { get; set; }

        [Range(1, 6)]
        public int Difficulty { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

      //  public int HikeStartPointId { get; set; }

     //   public virtual HikeStartPoint HikeStartPoint { get; set; }

    //    public int HikeEndPointId { get; set; }

     //   public virtual HikeEndPoint HikeEndPoint { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public int MountainId { get; set; }

      //  public IEnumerable<IFormFile> HikeImages { get; set; }

        //   public virtual IEnumerable<HikeImageInputModel> HikeImages { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RegionsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> MountainsItems { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hike, EditHikekInputModel>()
                .ForMember(x => x.Duration, opt =>
                    opt.MapFrom(x => (int)x.Duration.TotalMinutes));
                 //.ForMember(x => x.HikeStartPoint, opt =>
                 //   opt.MapFrom(x => x.HikeStartPoint))
                 //.ForMember(x => x.HikeEndPoint, opt =>
                 //    opt.MapFrom(x => x.HikeEndPoint))
                 //  .ForMember(x => x.HikeStartPointId, opt =>
                 //   opt.MapFrom(x => x.HikeStartPointId))
                 //.ForMember(x => x.HikeEndPointId, opt =>
                 //    opt.MapFrom(x => x.HikeEndPointId));
        }
    }
}
