using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
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
  // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {  id = RouteParameter.Optional }
            );
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
