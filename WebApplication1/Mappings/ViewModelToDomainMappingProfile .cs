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
            CreateMap<Food, FoodViewModel>().ForMember(f => f.FoodId, map => map.MapFrom(vm => vm.Id))
                .ForMember(f => f.FoodName, map => map.MapFrom(vm => vm.Name))
                .ForMember(f => f.FoodCarboHydrates, map => map.MapFrom(vm => vm.Hydrates))
                .ForMember(f => f.KCalory, map => map.MapFrom(vm => vm.KCalory))
                .ForMember(f => f.Fats, map => map.MapFrom(vm => vm.Fats))
                .ForMember(f => f.Protein, map => map.MapFrom(vm => vm.Protein))


            CreateMap<Meal, MealViewModel>().ForMember(vm => vm.Name, map => map.MapFrom(m => m.Name))
                .ForMember(vm => vm.MealDate, map => map.MapFrom(m => m.SetDate))
                .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id));
                
                
            CreateMap<User, UserMealsViewModel>().ForMember(u => u.Name, map => map.MapFrom(vm => vm.UserName));

        }
    }
}