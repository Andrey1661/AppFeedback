using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppFeedBack
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Store",
                url: "Feedback/Store/",
                defaults: new { controller = "FeedBack", action = "StoreFeedback" }
            );

            routes.MapRoute(
                name: "View",
                url: "Feedback/View/",
                defaults: new { controller = "FeedBack", action = "ViewFeedbacks" }
                );

            routes.MapRoute(
                name: "Delete",
                url: "Feedback/Delete",
                defaults: new { controller = "FeedBack", action = "DeleteFeedback" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "FeedBack", action = "ViewFeedbacks" }
            );
        }
    }
}
