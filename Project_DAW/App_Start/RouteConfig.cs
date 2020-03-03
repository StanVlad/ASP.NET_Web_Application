using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_DAW
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewArticle",
                url: "Articles/New",
                defaults: new { controller = "Article", action = "New" }
            );

            routes.MapRoute(
                name: "ShowArticle",
                url: "Articles/Show/{Id}",
                defaults: new { controller = "Article", action = "Show" }
            );

            routes.MapRoute(
                name: "EditArticle",
                url: "Articles/Edit/{Id}",
                defaults: new { controller = "Article", action = "Edit" }
            );

            routes.MapRoute(
                name: "DeleteArticle",
                url: "Articles/Delete/{title}",
                defaults: new { controller = "Article", action = "Delete" }
            );

            routes.MapRoute(
                name: "RollbackReplace",
                url: "Articles/RollbackReplace/{artTitle}/{artId}",
                defaults: new { controller = "Article", action = "RollbackReplace" }
            );

            /*
            routes.MapRoute(
                name: "RollbackDelete",
                url: "Article/RollbackDelete/{currentId}/{previousId}",
                defaults: new { controller = "Article", action = "RollbackDelete" }
            );
            */

            routes.MapRoute(
                name: "ArticleList",
                url: "Articles",
                defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "ShowCategory",
                url: "Categories/Show/{Id}",
                defaults: new { controller = "Category", action = "Show" }
            );

            routes.MapRoute(
                name: "NewCategory",
                url: "Categories/New",
                defaults: new { controller = "Category", action = "New" }
            );

            routes.MapRoute(
                name: "ListCategories",
                url: "Categories",
                defaults: new { controller = "Category", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
