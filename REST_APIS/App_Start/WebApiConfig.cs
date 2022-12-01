using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace REST_APIS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}/{sec_param}/{third_param}/{fourth_param}",
                defaults: new { id = RouteParameter.Optional ,sec_param = RouteParameter.Optional, third_param = RouteParameter.Optional, fourth_param = RouteParameter.Optional }
            );
        }
    }
}
