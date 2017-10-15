using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebApplication1.Identity;
using WebApplication1.Infrastructure;
using WebApplication1.Mappings;
using WebApplication1.Providers;
using WebApplication1.Service;

namespace WebApplication1
{
    public class DependencyContainer
    {
     /*   internal static IContainer Initialize(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterModule(new AutoMapperModule());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<ApplicationUser, Guid>>().InstancePerRequest();
            builder.RegisterType<UserManager<ApplicationUser, Guid>>().AsSelf().SingleInstance();
            builder.RegisterType<CustomOAuthProvider>().AsSelf().SingleInstance();
            builder.Register(context => context.Resolve<MapperConfiguration>().CreateMapper(context.Resolve)).As<IMapper>()
                .InstancePerLifetimeScope();
            // Repositories
            builder.RegisterAssemblyTypes(typeof(FoodRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            // Services
            builder.RegisterAssemblyTypes(typeof(FoodService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            return builder.Build();

        } */
    }
}