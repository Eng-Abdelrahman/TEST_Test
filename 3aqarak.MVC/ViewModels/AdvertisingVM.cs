using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class AdvertisingVM
    {
        public int Available_Id { get; set; }

        public int AvailableCat { get; set; } 

        public int ClientId { set; get; }

        public string RegionName { get; set; }

        public string ClientName { get; set; }            
        // new prop for adv vm
        public decimal Space { get; set; }

        public int Floor { get; set; }

        public decimal Price { get; set; }

        public int Rooms { get; set; }

        public int Bathrooms { get; set; }

        public int NoOfElevator { set; get; }

        public string Type { get; set; }

        public SelectList Regions { get; set; }
    }
}