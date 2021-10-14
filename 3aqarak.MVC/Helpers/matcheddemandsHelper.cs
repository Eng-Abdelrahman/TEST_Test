using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Helpers
{
    public class MatchedDemandsHelper
    {
        public int FK_DemandUnits_Users_CreatedBy { get; set; }
        public int PK_DemandUnits_Id { get; set; }
        public int FK_ShopDemands_Users_CreatedBy { get; set; }
        public int PK_ShopDemands_Id { get; set; }
        public int FK_VillasDemands_Users_CreatedBy { get; set; }
        public int PK_VillasDemands_Id { get; set; }
        public int FK_LandsDemands_Users_CreatedBy { get; set; }
        public int PK_LandsDemands_Id { get; set; }
    }
}