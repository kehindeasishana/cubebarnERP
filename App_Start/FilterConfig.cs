using System.Web;
using System.Web.Mvc;
using Web.Attributes;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
        }
    }
}
