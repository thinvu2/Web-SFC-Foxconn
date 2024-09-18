using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SN_API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default11",
                url: "{controller}/{action}/{sn}",
                defaults: new { controller = "Get", action = "GetData", sn = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default12",
                url: "{controller}/{action}/{dynamic}",
                defaults: new { controller = "Get", action = "GetRoute", dynamic = UrlParameter.Optional }
            );
        }
    }
}
