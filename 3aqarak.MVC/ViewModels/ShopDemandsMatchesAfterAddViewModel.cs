using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class ShopDemandsMatchesAfterAddViewModel
    {
        public List<ShopDemandViewModel> ShopDemands { get; set; }

        public List<ShopDemandViewModel> ShopDemandsWithPreviews { get; set; }

        public ShopDemandViewModel ShopDemandsWithSameClient { get; set; }

        public ShopAvailableViewModel ShopAvailable { get; set; }

        public ClientsViewModel Seller { get; set; }
        public ClientsViewModel Buyer { get; set; }

        public ShopDemandsMatchesAfterAddViewModel()
        {
            ShopDemands = new List<ShopDemandViewModel>();
            ShopDemandsWithPreviews = new List<ShopDemandViewModel>();
        }
    }
}