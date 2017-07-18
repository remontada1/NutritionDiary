using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
namespace WebApplication1.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
                {
                    x.AddProfile<DomainToViewModelMappingFood>();
                    x.AddProfile<ViewModelToDomainMappingFood>();
                });
        }

    }
}