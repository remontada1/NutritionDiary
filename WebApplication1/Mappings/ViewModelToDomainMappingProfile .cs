using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.ViewModels;
using AutoMapper;
using WebApplication1.Models;
namespace WebApplication1.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<FoodViewModel, Food>()
                .ForMember(f => f.Name, map => map.MapFrom(vm => vm.Name))
                .ForMember(f => f.Hydrates, map => map.MapFrom(vm => vm.Hydrates))
                .ForMember(f => f.KCalory, map => map.MapFrom(vm => vm.KCalory))
                .ForMember(f => f.Fats, map => map.MapFrom(vm => vm.Fats))
                .ForMember(f => f.Protein, map => map.MapFrom(vm => vm.Protein))
                .ForMember(f => f.Image, map => map.MapFrom(vm => vm.Image)));
        }
    }
}