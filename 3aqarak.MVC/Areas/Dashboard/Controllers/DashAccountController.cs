//using _3aqarak.BLL.Dto;
//using _3aqarak.BLL.Helpers;
//using _3aqarak.BLL.Interfaces;
//using _3aqarak.MVC.Areas.Dashboard.ViewModels;
//using _3aqarak.MVC.Filters;
//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;


//namespace _3aqarak.MVC.Areas.Dashboard.Controllers
//{
//    // GET: Dashboard/Account

//    public class DashAccountController : Controller
//    {
//        private IUSerService _userService;
//        private IConfirmation _conf;

//        public DashAccountController(IUSerService userServive, IConfirmation conf)
//        {
//            _userService = userServive;
//            _conf = conf;
//        }
      

//        [LoggedInFilter]
//        //
//        // POST: /Account/Login
//        public ActionResult Login()
//        {
//            if (HttpContext.Items["msg"] != null)
//            {
//                Session["message"] = HttpContext.Items["msg"].ToString();
//                return RedirectToAction("users", "admin", new { area = "dashboard" });
//            }
//            ViewBag.MessageColor = "#666";
//            ViewBag.Message = "Sign in to start your session";
//            ViewBag.ReturnURL = Request.QueryString["returnurl"];

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Login(string userName, string password,bool?rememberMe)
//        {
//            ViewBag.MessageColor = "#f00";
//            var result = await _userService.Login(userName, password,rememberMe);
//            if (result.LoginStatus == Status.Succeeded)
//            {
//                if (Request.QueryString["returnurl"] != null)
//                {
//                    return Redirect(Request.QueryString["returnurl"]);
//                }
//                if (Session["Admin"]!=null)
//                {
//                    var sessin = (UserDto)Session["Admin"];
//                }
//                return RedirectToAction("Index", "dashboard", new { area = "dashboard" });
//            }
//            ViewBag.Message = result.Message;

//            return View();
//        }

//        public ActionResult Logout()
//        {
//            Session["Admin"] = null;
//            return RedirectToAction("Login", "Account", new { area = "dashboard" });
//        }





//    }





//}