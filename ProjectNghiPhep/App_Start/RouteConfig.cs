using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectNghiPhep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Default", // Route name  
                url: "{controller}/{action}/{id}.cshtml", // URL with parameters  
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults  
            );
        }
    }
}