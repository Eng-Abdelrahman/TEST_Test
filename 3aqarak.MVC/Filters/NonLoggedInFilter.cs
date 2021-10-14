using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Services;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace _3aqarak.MVC.Filters
{
    public class NonLoggedInFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private IUSerService _userService;

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            _userService = DependencyResolver.Current.GetService<IUSerService>();
            UserDto user = null;
            bool skipAuthorization = filterContext.
                                                    ActionDescriptor.
                                                    IsDefined(typeof(AllowAnonymousAttribute), true)
                                                    || filterContext.ActionDescriptor.
                                                     ControllerDescriptor.
                                                    IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
            {
                return;
            }

            var rememberMeToken = filterContext.HttpContext.Request.Cookies["Rck"]?["Rtkn"];
            if (rememberMeToken != null)
            {
                user = _userService.FindByRtknWithoutAsync(rememberMeToken);

            }

            if (filterContext.HttpContext.Session["User"] == null)
            {
                if (user == null)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.HttpContext.Session["User"] = user;

                }

            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary{
                        {"area","" },
                        { "controller", "Account" },
                        { "action", "Login" },
                        { "returnurl", filterContext.HttpContext.Request.RawUrl }
                    });
            }


        }
    }

}



//public class NonLoggedInFilter : AuthorizeAttribute
//{
//    private IUSerService _userService;

//    public override void OnAuthorization(AuthorizationContext filterContext)
//    {

//        _userService = DependencyResolver.Current.GetService<IUSerService>();
//        UserDto user=null;

//        bool skipAuthorization = filterContext.
//                                                ActionDescriptor.
//                                                IsDefined(typeof(AllowAnonymousAttribute), true)
//                                                || filterContext.ActionDescriptor.
//                                                 ControllerDescriptor.
//                                                IsDefined(typeof(AllowAnonymousAttribute), true);

//        if (skipAuthorization)
//        {
//            return;
//        }

//        var rememberMeToken = filterContext.HttpContext.Request.Cookies["Rck"]?["Rtkn"];

//        if (rememberMeToken != null)
//        {
//             user = _userService.FindByRtkn(rememberMeToken);

//        }

//        if (filterContext.HttpContext.Session["User"] == null)
//        {
//            if (user==null)
//            {
//                filterContext.Result = new RedirectToRouteResult(
//                    new RouteValueDictionary{
//                        { "controller", "Account" },
//                        { "action", "Login" },                           
//                        { "returnurl", filterContext.HttpContext.Request.RawUrl }
//                    });
//            }
//            else
//            {
//                filterContext.HttpContext.Session["User"] = user;     

//            }

//        }

//        //base.OnAuthorization(filterContext);
//    }
//}