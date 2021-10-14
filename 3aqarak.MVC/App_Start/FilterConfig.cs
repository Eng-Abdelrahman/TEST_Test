using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Filters;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NonLoggedInFilter());
        }
    }
}
