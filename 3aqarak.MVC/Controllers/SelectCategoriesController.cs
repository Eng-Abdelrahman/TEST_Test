using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Controllers
{
    public class SelectCategoriesController : Controller
    {    
        // GET: SelectCategories
        public ActionResult AvailableCat()
        {
            return View();
        }

        public ActionResult DemandCat()
        {
            return View();
        }
    }
}