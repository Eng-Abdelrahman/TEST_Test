using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using System.Web.Mvc;

namespace _3aqarak.MVC.Filters
{
    public class IsAdminFilter: AuthorizeAttribute
    {

        private IUSerService _userService;
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _userService = DependencyResolver.Current.GetService<IUSerService>();
            var user = filterContext.HttpContext.Session["User"] != null ? (UserDto)filterContext.HttpContext.Session["User"] : new UserDto();         
            var newUser = _userService.FindUserByIDWithoutAsync(user.PK_Users_Id);
            if (!newUser.IsAdmin||!newUser.IsOwner)
            {
                filterContext.HttpContext.Items["msg"] = "مستخدم غير مصرح!";
            }

            //base.OnActionExecuting(filterContext);
        }
    }
}