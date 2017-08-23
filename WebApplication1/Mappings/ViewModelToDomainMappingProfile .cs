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

        protected override void Configure()
        {
            CreateMap<Food, FoodViewModel>().ForMember(f => f.FoodName, map => map.MapFrom(vm => vm.Name))
                .ForMember(f => f.FoodCarboHydrates, map => map.MapFrom(vm => vm.Hydrates))
                .ForMember(f => f.KCalory, map => map.MapFrom(vm => vm.KCalory))
                .ForMember(f => f.Fats, map => map.MapFrom(vm => vm.Fats))
                .ForMember(f => f.Protein, map => map.MapFrom(vm => vm.Protein))
                .ForMember(f => f.Image, map => map.MapFrom(vm => vm.Image));

            CreateMap<Meal, MealViewModel>().ForMember(m => m.MealName, map => map.MapFrom(vm => vm.Name))
                .ForMember(m=> m.Foods, map => map.MapFrom(vm => vm.Foods.ToList()));
            CreateMap<Food, MealViewModel>().ForMember(f => f.FoodName, map => map.MapFrom(vm => vm.Name))
                .ForMember(f => f.KCalories, map => map.MapFrom(vm => vm.KCalory));

        }
    }
}