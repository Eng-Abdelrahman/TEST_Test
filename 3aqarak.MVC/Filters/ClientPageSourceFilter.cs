using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Filters
{
    public class ClientPageSourceFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer == null)
            {
                return;
            }
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString().Split('/').Contains("clientSales"))
            {
                filterContext.HttpContext.Items["RedirectUrl"] = "/clientSales/index";
            }
            else if (filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString().Split('/').Contains("clientdemand"))
            {
                filterContext.HttpContext.Items["RedirectUrl"] = "/clientdemand/index";
            }
            else
            {
                filterContext.HttpContext.Items["RedirectUrl"] = "/dashboard/client/clients";
            }
           
           
        }
    }
}