using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using WebApplication1.Resolver;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.Service;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;


namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IDbFactory, DbFactory>(new HierarchicalLifetimeManager());
            container.RegisterType<IFoodRepository, FoodRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFoodService, FoodService>(new HierarchicalLifetimeManager());


            config.DependencyResolver = new UnityResolver(container);
             
          


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Customer",
                routeTemplate: "api/customer/{id}",
                defaults: new { controller = "customer", id = RouteParameter.Optional }
            );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
