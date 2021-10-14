using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.MVC.Helpers;
using _3aqarak.MVC.Hubs;
using _3aqarak.MVC.ViewModels;

using AutoMapper;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace _3aqarak.MVC.Controllers
{
    public class DemandMatchesNotificationsController : Controller
    {
        private readonly IAvailableService _availableService;
        private readonly IVillasAvailablesService _villasAvailable;
        private readonly IAvailableLandsSevice _availableLand;
        private readonly IShopAvailableService _shopAvailable;

        public DemandMatchesNotificationsController(IAvailableService availableService, IVillasAvailablesService villasAvailable, IAvailableLandsSevice availableLand, IShopAvailableService shopAvailable)
        {
            _availableService = availableService;
            _villasAvailable = villasAvailable;
            _availableLand = availableLand;
            _shopAvailable = shopAvailable;
        }

        //public ActionResult ShowDemandMatchesNotifications()
        //{
        //    return View();
        //}

        // GET: DemandMatchesNotifications
        public ActionResult ShowDemandMatchesNotifications(string availables, string demands, string categories)
        {
            if (!string.IsNullOrEmpty(demands))
            {
                ViewBag.demands = JsonConvert.DeserializeObject<List<List<int>>>(demands);
            }
            if (!string.IsNullOrEmpty(availables))
            {
                ViewBag.availables = JsonConvert.DeserializeObject<List<int>>(availables);
            }
            if (!string.IsNullOrEmpty(categories))
            {
                ViewBag.categories = JsonConvert.DeserializeObject<List<int>>(categories);

            }
            return View();

        }

    }
}