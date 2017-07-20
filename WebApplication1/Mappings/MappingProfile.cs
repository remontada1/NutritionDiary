using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WebApplication1.ViewModels;
using WebApplication1.Models;

namespace WebApplication1.Mappings
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Food, FoodViewModel>();

        }
    }
}