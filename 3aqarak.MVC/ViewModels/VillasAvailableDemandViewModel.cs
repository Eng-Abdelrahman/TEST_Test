
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _3aqarak.MVC.ViewModels
{
    public class VillasAvailableDemandViewModel
    {
        public VillaClientDemandViewModel Demand { get; set; }

        public List<VillaClientDemandViewModel> Demands { get; set; }

        public List<VillaClientDemandViewModel> DemandsWithPreviews { get; set; }

        public VillaClientDemandViewModel DemandsWithSameClient { get; set; }

        public List<VillsAvailableViewModel> availables { get; set; }

        public VillsAvailableViewModel available { get; set; }
        public ClientsViewModel Seller { get; set; }

        public ClientsViewModel Buyer { get; set; }

        public VillsAvailableViewModel SameClientAvailable { get; set; }

        public List<VillsAvailableViewModel> ExcludedAvailablesForPreviws { get; set; }

        public List<DateTime> Dates { get; set; }
        

        public VillasAvailableDemandViewModel()
        {          
            DemandsWithPreviews = new List<VillaClientDemandViewModel>();
            availables = new List<VillsAvailableViewModel>();
            ExcludedAvailablesForPreviws = new List<VillsAvailableViewModel>();
            Dates = new List<DateTime>();
            Demands = new List<VillaClientDemandViewModel>();
        }
    }
}