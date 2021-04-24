using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ZigmaWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Attribute routing.
            config.MapHttpAttributeRoutes();

            // Convention-based routing.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register WebApi Not Implemented Exceptions Filter Attribute 
            config.Filters.Add(new Controllers.BaseApiController.NotImplExceptionFilterAttribute());
        }
    }
}
