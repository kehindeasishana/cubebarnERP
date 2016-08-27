using SimpleMvcUserManagement;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            UserManagementController.RegisterMe();

            routes.MapRoute(
                name: "DefaultAction",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{id}",
            //    defaults: new { id = UrlParameter.Optional }
            //);
        }

        protected void Application_AuthenticateRequest()
        {
            // check if the user management controller is the target controller for this request
            if (UserManagementController.IsTargeted())
            {
                // Do any custom authorization checks here (e.g. Roles.IsUserInRole("Admin")) 
                UserManagementController.IsRequestAuthorized = true;
            }
        }
    }
}
