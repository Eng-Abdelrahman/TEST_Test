using System;
using System.Collections.Generic;

namespace _3aqarak.MVC.ViewModels
{
    public class DemandAvailableMatchViewModel
    {
        public List<AvailableViewModel> availables { get; set; }

        public List<AvailableViewModel> ExcludedavailablesForPreviws { get; set; }

        public AvailableViewModel SameClientAvailable { get; set; }

        public List<DateTime> Dates { get; set; }

        public DemandViewModel Demand { get; set; }

        public ClientsViewModel Buyer { get; set; }

        public DemandAvailableMatchViewModel()
        {
            availables = new List<AvailableViewModel>();
            ExcludedavailablesForPreviws = new List<AvailableViewModel>();
            Dates = new List<DateTime>();
        }
    }
}