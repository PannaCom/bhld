using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QueenLand
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            //routes.MapRoute(
            //    "view project",
            //    "{fromdate}-{todate}-{name}-{rate}-{dis}-page{page}",
            //    new { controller = "Search", action = "SearchHotel", fromdate = UrlParameter.Optional, todate = UrlParameter.Optional, name = UrlParameter.Optional, rate = UrlParameter.Optional, dis = UrlParameter.Optional, page = UrlParameter.Optional }
            //);
            routes.MapRoute(
                "view project item page",
                "{name}-{id}",
                new { controller = "projectcontent", action = "SinglePage", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "view project list",
                "danh-sach-san-pham/{name}-{id}",
                new { controller = "projects", action = "list", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "view project list item",
                "danh-sach-san-pham-con/{projectname}/{projectitemname}-{id}",
                new { controller = "projectitem", action = "list", projectname = UrlParameter.Optional, projectitemname = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    "view project",
            //    "danh-sach-san-pham/{name}-{id}",
            //    new { controller = "projects", action = "Details", name = UrlParameter.Optional, id = UrlParameter.Optional}
            //);
            routes.MapRoute(
                "view news",
                "tin",
                new { controller = "news", action = "List"}
            );
            routes.MapRoute(
                "view detail news",
                "tin/{name}-{id}",
                new { controller = "news", action = "GetDetails", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "view jobs",
                "jobs/list",
                new { controller = "jobs", action = "List" }
            );
            routes.MapRoute(
                "view detail jobs",
                "jobs/details/{name}-{id}",
                new { controller = "jobs", action = "GetDetails", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );  
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}