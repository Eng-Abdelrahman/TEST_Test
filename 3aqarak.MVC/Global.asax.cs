using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace _3aqarak.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => cfg.AddProfiles(typeof(MvcApplication).Assembly));
            

        }
        protected void Session_Start(Object sender, EventArgs e)
        { 
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            if (Session["contractImagePath"] != null)
            {
                var paths = (List<string>)Session["contractImagePath"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/ContractImage"), path.Split('/')[2], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }
            }

            if (Session["UsersImagePathList"] != null)
            {
                var photo = Directory
                    .GetFiles(Server.MapPath("/Assets/Img/Users"), (Session["UsersImagePathList"]
                    .ToString()).Split('/')[3], SearchOption.AllDirectories)
                    .FirstOrDefault();
                if (photo != null)
                {
                    System.IO.File.Delete(photo);
                }
            }

            if (Session["clientImagePathList"] != null)
            {
                var paths = (List<string>)Session["clientImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/Clients"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["ClientSalesImagePathList"] != null)
            {
                var paths = (List<string>)Session["ClientSalesImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/ClientSalesImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["SaleContractImagePathList"] != null)
            {
                var paths = (List<string>)Session["SaleContractImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/SaleContractImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["RentContractImagePathList"] != null)
            {
                var paths = (List<string>)Session["RentContractImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/RentContractImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["AvailableLandsImagePathList"] != null)
            {
                var paths = (List<string>)Session["AvailableLandsImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/AvailableLandsImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["AvailableShopImagePathList"] != null)
            {
                var paths = (List<string>)Session["AvailableLandsImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/AvailableShopImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

            if (Session["VillasClientSalesImagePathList"] != null)
            {
                var paths = (List<string>)Session["VillasClientSalesImagePathList"];
                foreach (var path in paths)
                {
                    var photo = Directory
                             .GetFiles(Server.MapPath("/Assets/Img/ClientSalesImages"), path.Split('/')[3], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }

            }

        }
    }


}
