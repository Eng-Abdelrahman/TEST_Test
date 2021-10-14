using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class ShopAvailableDemandViewModel
    {
        public ShopDemandViewModel Demand { get; set; }

        public List<ShopDemandViewModel> Demands { get; set; }

        public List<ShopDemandViewModel> DemandsWithPreviews { get; set; }

        public ShopDemandViewModel DemandsWithSameClient { get; set; }

        public List<ShopAvailableViewModel> availables { get; set; }

        public ShopAvailableViewModel available { get; set; }
        public ClientsViewModel Seller { get; set; }

        public ClientsViewModel Buyer { get; set; }

        public ShopAvailableViewModel SameClientAvailable { get; set; }

         public List<ShopAvailableViewModel> ExcludedAvailablesForPreviws { get; set; }

        public List<DateTime> Dates { get; set; }


        public ShopAvailableDemandViewModel()
        {
            DemandsWithPreviews = new List<ShopDemandViewModel>();
            availables = new List<ShopAvailableViewModel>();
            ExcludedAvailablesForPreviws = new List<ShopAvailableViewModel>();
            Dates = new List<DateTime>();
            Demands = new List<ShopDemandViewModel>();
            //Demand = new ShopDemandViewModel();
            //SameClientAvailable = new ShopAvailableViewModel();
            
        }
    }
}