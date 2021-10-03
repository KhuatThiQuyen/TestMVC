using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace School_Manager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Student Edit",
                url: "thong-tin-hoc-sinh/{Name}/{id}",
                defaults: new { controller = "Students", action = "Edit", id = UrlParameter.Optional }
                //namespaces: new string[] { "School_Manager.Controllers" }
            );
            routes.MapRoute(
                name: "Student Details",
                url: "bang-diem-hoc-sinh/{Name}/{id}",
                defaults: new { controller = "Students", action = "Details", id = UrlParameter.Optional }
                //namespaces: new string[] { "School_Manager.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                //link url domain mặc định 
                url: "{controller}/{action}/{id}",
                //comeback page index
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            

        }
    }
}
