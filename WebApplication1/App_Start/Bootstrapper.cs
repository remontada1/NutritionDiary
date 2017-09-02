using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Extras.NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Infrastructure;
using WebApplication1.Service;
using AutoMapper;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;
using Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using WebApplication1.Models;
using WebApplication1.DAL;
using System.Threading.Tasks;

namespace WebApplication1.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            SetNLogConfig();
        }

        private static void SetAutofacContainer()
        {

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.Register(c=> new UserStore<User>(c.Resolve<CustomerContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>());
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
{
    DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Application​")
}); 


            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
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

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); 
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);

        }

        private static void SetNLogConfig()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<NLogModule>();
        }
    }
}