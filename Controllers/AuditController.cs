using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class AuditController : Controller
    {
        // GET: Audit
        public ActionResult ViewAuditRecords()
        {
            var audits = new ApplicationContext().Audit.OrderByDescending(a => a.TimeAccessed);

            return View(audits);
        }
    }
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;

            //Generate an audit
            Audit audit = new Audit()
            {
                AuditID = Guid.NewGuid(),
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                UrlAccessed = request.RawUrl,
                TimeAccessed = DateTime.UtcNow,
                UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous",
            };

            //Stores the Audit in the Database
            ApplicationContext context = new ApplicationContext();
            context.Audit.Add(audit);
            context.SaveChanges();

            base.OnActionExecuting(filterContext);
        }
    }
}