using System;
using Microsoft.Owin;
using Owin;
using NLog.Owin.Logging;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;
using WebApplication1.Infrastructure;
using WebApplication1.Service;
using AutoMapper;
using WebApplication1.Mappings;
using Microsoft.AspNet.Identity;
using WebApplication1.Identity;
using Microsoft.Owin.Security.OAuth;
using WebApplication1.Providers;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {

        //public OAuthAuthorizationServerOptions OAuthAuthorizationServer { get; set; }
        //public UserManager<ApplicationUser, Guid> userManager { get; set; }

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
