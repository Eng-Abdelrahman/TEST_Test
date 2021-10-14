using _3aqarak.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Helpers
{
    public class MatchesForPreviewsHelper
    {
        public static AvailableDemandMatchViewModel UpdateDemandMatches { get; set; }
        public static AvailableDemandMatchViewModel AddDemandMatches { get; set; }
        // add here Villas Availableto match ************************* Mostafa ******************************
        public static VillasAvailableDemandViewModel UpdateVillasAvailableMatches { set; get; }
        public static VillasAvailableDemandViewModel AddVillasAvailableMatches { set; get; }

        public static DemandAvailableMatchViewModel UpdateAvailableMatches { get; set; }
        public static DemandAvailableMatchViewModel AddAvailableMatches { get; set; }
    }
}