using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class LandDemandMatchViewModel
    {
        public List<LandsDemandsViewModel> LandDemands { get; set; }

        public List<LandsDemandsViewModel> LandDemandsWithPreviews { get; set; }

        public LandsDemandsViewModel DemandsWithSameClient { get; set; }

        public AvailableLandsViewModel Landsavailable { get; set; }

        public ClientsViewModel Seller { get; set; }
        public ClientsViewModel Buyer { get;  set; }

        public LandDemandMatchViewModel()
        {
            LandDemands = new List<LandsDemandsViewModel>();
            LandDemandsWithPreviews = new List<LandsDemandsViewModel>();
        }
    }
}