using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private IUSerService _userService;
        private IConfirmation _conf;

        
        public AccountController(IUSerService userServive, IConfirmation conf)
        {
            _userService = userServive;
            _conf = conf;
        }

        [AllowAnonymous]
        [LoggedInFilter]
        //[GetNotifications]
        // POST: /Account/Login
        public ActionResult Login()
        {
            if (HttpContext.Items["msg"] != null)
            {
                Session["message"] = HttpContext.Items["msg"].ToString();
                return RedirectToAction("CustomErrorView", "Home", new { area = "" });
            }
            ViewBag.MessageColor = "#666";
            ViewBag.Message = "Sign in to start your session";
            ViewBag.ReturnURL = Request.QueryString["returnurl"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string userName, string password, bool? rememberMe)
        {
            //var settings = System.Configuration.ConfigurationManager.ConnectionStrings["RealEstateDB"];
            //var fi = typeof(System.Configuration.ConfigurationElement).GetField(
            //              "_bReadOnly",
            //              BindingFlags.Instance | BindingFlags.NonPublic);
            //fi.SetValue(settings, false);
            //settings.ConnectionString = "data source=.;initial catalog=RealEstate_V2DB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

            ViewBag.MessageColor = "#f00";
            var result = await _userService.Login(userName, password,rememberMe);
            if (result.LoginStatus == Status.Succeeded)
            {
                if (Request.QueryString["returnurl"] != null)
                {
                    return Redirect(Request.QueryString["returnurl"]);
                }
               
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = result.Message;

            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            var cookie = HttpContext.Request.Cookies["Rck"];
            if (cookie!=null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Set(cookie);
            }
            return RedirectToAction("Login", "Account");
        }

    }
}