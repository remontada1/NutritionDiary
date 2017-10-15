using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using System;
using System.Reflection;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WebApplication1.AttributeRoutingHttpConfig), "Start")]

namespace WebApplication1 
{
    public static class AttributeRoutingHttpConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes) 
		{
            try
            {
                routes.MapHttpAttributeRoutes();
            }
            catch (Exception ex)
            {
                if (ex is System.Reflection.ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }
            }
            // See http://github.com/mccalltd/AttributeRouting/wiki for more options.
            // To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            
		}

        public static void Start() 
		{
            RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}
