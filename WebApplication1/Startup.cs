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
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using System.Web.Configuration;

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

            ConfigureOAuthTokenConsumption(app);
            builder.RegisterType<CustomOAuthProvider>().As<IOAuthAuthorizationServerProvider>().InstancePerDependency();

            builder
                .Register(x => new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/oauth/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                    Provider = x.Resolve<IOAuthAuthorizationServerProvider>(),
                    AccessTokenFormat = new CustomJwtFormat("http://localhost:36290")
                })
                 .AsSelf()
                .InstancePerDependency();



            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            builder.RegisterType<OAuthAuthorizationServerMiddleware>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<ApplicationUser, Guid>>().InstancePerRequest();
            builder.RegisterType<RoleStore>().As<IRoleStore<Identity.IdentityRole, Guid>>();
            builder.RegisterType<UserManager<ApplicationUser, Guid>>();
            builder.RegisterType<RoleManager<Identity.IdentityRole, Guid>>();
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


        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "http://localhost:36290";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}
