using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SinavGorevlendirme
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("homepage_default", "yonetim", new { action = "Index", Controller = "Admin" });
            routes.MapRoute("loginpage", "yonetim/login", new { action = "Login", Controller = "Admin" });
            routes.MapRoute("homepage", "yonetim/anasayfa", new { action = "Index", Controller = "Admin" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}