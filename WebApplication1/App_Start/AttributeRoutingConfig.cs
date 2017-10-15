using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using System;
using System.Reflection;
using System.Net.Http;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WebApplication1.AttributeRoutingConfig), "Start")]

namespace WebApplication1 
{
    public static class AttributeRoutingConfig
	{
		public static void RegisterRoutes(RouteCollection routes) 
		{
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd
            try
            {
                routes.MapAttributeRoutes();
            }
            catch (Exception ex)
            {
                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }
            }
            
		}

        public static void Start() 
		{
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
