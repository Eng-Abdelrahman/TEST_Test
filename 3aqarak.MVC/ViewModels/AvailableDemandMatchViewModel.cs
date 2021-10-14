using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class AvailableDemandMatchViewModel
    {
        public List<DemandViewModel> Demands { get; set; }

        public List<DemandViewModel> DemandsWithPreviews { get; set; }

        public DemandViewModel DemandsWithSameClient { get; set; }

        public AvailableViewModel available { get; set; }

        public ClientsViewModel Seller { get; set; }
        public ClientsViewModel Buyer { get;  set; }

        public AvailableDemandMatchViewModel()
        {
            Demands = new List<DemandViewModel>();
            DemandsWithPreviews = new List<DemandViewModel>();
        }
    }
}