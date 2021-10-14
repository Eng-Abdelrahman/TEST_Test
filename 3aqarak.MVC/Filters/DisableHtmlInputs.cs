using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Filters
{
    public class DisableHtmlInputs:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer==null)
            {
                return;
            }
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString().Split('/').LastOrDefault().Contains("EndedRentalsIndex"))
            {
                filterContext.HttpContext.Items["Disable"] = true;
            }
            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString().Split('/').LastOrDefault().Contains("AdevertisingIndex"))
            {
                filterContext.HttpContext.Items["Disable"] = true;
            }
        }
    }
}