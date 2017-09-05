using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using WebApplication1.App_Start;
using NLog.Owin.Logging;
using NLog;
using Autofac.Integration.WebApi.Owin;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Extras.NLog;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Infrastructure;
using WebApplication1.Service;
using AutoMapper;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using WebApplication1.Models;
using WebApplication1.DAL;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.Register(c => new UserStore<User>(c.Resolve<CustomerContext>())).AsImplementedInterfaces().InstancePerRequest();
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


            var container = builder.Build();

            // This will add the Autofac middleware as well as the middleware
            // registered in the container.
            app.UseAutofacMiddleware(container);

            app.UseWebApi(config);
            app.UseAutofacWebApi(config);
            app.UseNLog();
            

            

        }
    }
}
