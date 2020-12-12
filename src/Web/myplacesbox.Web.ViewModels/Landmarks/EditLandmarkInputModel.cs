namespace MyPlacesBox.Web.ViewModels.Landmarks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using MyPlacesBox.Data.Models;
    using MyPlacesBox.Services.Mapping;

    public class EditLandmarkInputModel : IMapFrom<Landmark>//, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        // Global cordinates
        public double? Latitude { get; set; }

        public double? Longitute { get; set; }

        [MinLength(5)]
        public string Websate { get; set; }

        [MinLength(7)]
        public string PhoneNumber { get; set; }

        [MinLength(4)]
        public string WorkTime { get; set; }

        [MinLength(4)]
        public string DayOff { get; set; }

        public double EntranceFee { get; set; }

        [Required]
        [MinLength(100)]
        public string Description { get; set; }

        [Range(1, 6)]
        public int Stars { get; set; }

        [Range(1, 6)]
        public int Difficulty { get; set; }

     //   public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public int RegionId { get; set; }

        public int TownId { get; set; }

        public int MountainId { get; set; }

      //  public IEnumerable<IFormFile> LandmarkImages { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RegionsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TownsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> MountainsItems { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Landmark, EditLandmarkInputModel>()
        //        .ForMember(x => x.LandmarkImages, opt =>
        //            opt.MapFrom(x =>
        //            x.LandmarkImages));
        //}
    }
}
