using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _3aqarak.MVC.Filters
{
    public class IsBranchManager:AuthorizeAttribute
    {
        private IUSerService _userService;
        private IBranchService _branchService;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _userService = DependencyResolver.Current.GetService<IUSerService>();
            _branchService = DependencyResolver.Current.GetService<IBranchService>();
            if (filterContext.HttpContext.Session["User"]!=null)
            {
                var user = (UserDto)filterContext.HttpContext.Session["User"];

                var branchMgr = (_branchService.FindByIDWithoutAsync(user.FK_Users_Branches_Id)).FK_Branches_Users_MgrId;
                if ((user.IsOwner||branchMgr==user.PK_Users_Id)&&user.IsActive)
                {
                    return;
                }
                else
                {
                    filterContext.HttpContext.Items["msg"] = "مستخدم غير مصرح!";
                }
              
            }
        }
    }
}