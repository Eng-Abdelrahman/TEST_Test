using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _3aqarak.MVC.Filters
{
    public class LoggedInFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] != null)
            {

                filterContext.HttpContext.Items["msg"] = "Your've already logged in!";
            }
            else
            {

            }


        }
    }
}