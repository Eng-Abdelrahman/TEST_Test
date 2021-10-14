using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class DemandGetDataViewModel
    {
        public int PK_DemandUnits_Id { get; set; }

        //public string ClientName { get; set; }

        //public string ShortDescription { get; set; }

        public string Type { get; set; }

        //public string DateString { get; set; }

        public int FK_DemandUnits_Clients_ClientId { get; set; }

        public int FK_DemandUnits_Categories_Id { get; set; }

        public int FK_DemandUnits_Transactions_Id { get; set; }

        public string RegionNameFrom { get; set; }

        public string RegionNameTo { get; set; }

        public int FK_DemandUnits_PaymentMethod_Id { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public int MinBathRooms { get; set; }

        public int MaxBathRooms { get; set; }

        public int MinRooms { get; set; }

        public int MaxRooms { get; set; }

        public int MinFloor { get; set; }

        public int MaxFloor { get; set; }

        public bool IsFurnished { get; set; }

        // change the name here 

        public int FK_DemandUnits_Users_Sales { get; set; }

        public string BuyerName { get; set; }

      
        public bool IsResidential { get; set; }

        public DateTime DateOfBuildFrom { get; set; }


        public DateTime DateOfBuildTo { get; set; }


        public int NoElevatorsFrom { get; set; }


        public int NoElevatorsTo { get; set; }
    }
}