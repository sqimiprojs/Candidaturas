using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Candidaturas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewUser",
                url: "{User}/{action}",
                defaults: new { controller = "User", action = "NewUser" }
            );
            routes.MapRoute(
                name: "RecoverPassword",
                url: "{User}/{action}",
                defaults: new { controller = "User", action = "RecoverPassword" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
