using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _3aqarak.MVC.Filters
{
    public class AdminChartsFilter : AuthorizeAttribute
    {
        private IUSerService _userService;
        public  override void OnAuthorization(AuthorizationContext filterContext)
        {
            _userService = DependencyResolver.Current.GetService<IUSerService>();
            var user = filterContext.HttpContext.Session["User"] != null ? (UserDto)filterContext.HttpContext.Session["User"] : new UserDto();
            var newUser =  _userService.FindUserByIDWithoutAsync(user.PK_Users_Id);
            if (!newUser.IsAdmin || !newUser.IsOwner)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary{
                        {"area","" },
                        { "controller", "Home" },
                        { "action", "EmpIndex" }
                    });
            }

            //base.OnActionExecuting(filterContext);
        }
    }
}