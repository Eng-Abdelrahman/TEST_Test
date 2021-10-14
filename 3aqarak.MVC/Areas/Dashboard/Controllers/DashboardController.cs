using _3aqarak.MVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.Controllers
{
    [IsAdminFilter]
    public class DashboardController : Controller
    {
        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {           
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home",new { area=""});                
            }
            return View();
        }
    }
}