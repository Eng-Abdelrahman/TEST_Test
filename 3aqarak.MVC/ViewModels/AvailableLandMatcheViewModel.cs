using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class AvailableLandMatcheViewModel
    {
        public List<AvailableLandsViewModel> availableLands { get; set; }

        public AvailableLandsViewModel availableLand { get; set; }
        public List<LandsDemandsViewModel> Demands { get; set; }

        public List<LandsDemandsViewModel> DemandsWithPreviews { get; set; }

        public LandsDemandsViewModel DemandsWithSameClient { get; set; }

        public List<AvailableLandsViewModel> excludedAvailablesLandForPreviews { get; set; }

        public AvailableLandsViewModel availableLandWithSameClient { get; set; }

        public LandsDemandsViewModel landDemand { get; set; }

        public ClientsViewModel Buyer { get; set; }

        public ClientsViewModel Seller { get; set; }

        public List<DateTime> Dates { get; set; }


        public AvailableLandMatcheViewModel()
        {
            availableLands = new List<AvailableLandsViewModel>();
            excludedAvailablesLandForPreviews = new List<AvailableLandsViewModel>();
            Dates = new List<DateTime>();
            DemandsWithPreviews = new List<LandsDemandsViewModel>();
            Demands = new List<LandsDemandsViewModel>();

            availableLand = new AvailableLandsViewModel();
        }
    }
}