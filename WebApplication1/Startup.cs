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
using WebApplication1.Repository;
using WebApplication1.Identity;
using Microsoft.Owin.Security.OAuth;
using WebApplication1.Providers;
using System.Diagnostics;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {

        public static OAuthAuthorizationServerOptions OAuthAuthorizationServer { get; set; }
        public static UserManager<ApplicationUser, Guid> userManager { get; set; }

        public void Configuration(IAppBuilder app)
        {
            var config = WebApiConfig.Register();

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new AutoMapperModule());
            builder.RegisterInstance(app).As<IAppBuilder>();

            builder.RegisterType<CustomOAuthProvider>().As<IOAuthAuthorizationServerProvider>().InstancePerDependency();

            builder
                .Register(x => new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/oauth/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                    Provider = x.Resolve<IOAuthAuthorizationServerProvider>()
                })
                 .AsSelf()
                .InstancePerDependency();

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            builder.RegisterType<OAuthAuthorizationServerMiddleware>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<ApplicationUser, Guid>>().InstancePerRequest();
            builder.RegisterType<UserManager<ApplicationUser, Guid>>();
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
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseWebApi(config);
            app.UseAutofacWebApi(config);
            app.UseNLog();

        }
    }
}
