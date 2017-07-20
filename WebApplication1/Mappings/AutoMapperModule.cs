using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WebApplication1.ViewModels;
namespace WebApplication1.Mappings
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Mapper.Initialize(cfg =>
                {
                    cfg.AddProfile<ViewModelToDomainMappingProfile>();
                    cfg.AddProfile<DomainToViewModelMappingProfile>();

                });
            
            base.Load(builder);
        }

    }
}