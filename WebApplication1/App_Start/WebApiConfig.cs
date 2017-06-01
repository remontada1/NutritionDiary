using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Practices.Unity;
using WebApplication1.Resolver;
using WebApplication1.Repository;
using WebApplication1.Models;
using System.Net.Http.Headers;


namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Customer",
                routeTemplate: "api/customer/{id}",
                defaults: new { controller = "customer", id = RouteParameter.Optional }
            );
        }
    }
}
