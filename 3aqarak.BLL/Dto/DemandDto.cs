using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
     public class DemandDto
    {
        public int PK_DemandUnits_Id { get; set; }

        public int FK_DemandUnits_Clients_ClientId { get; set; }

        public int FK_DemandUnits_Users_CreatedBy { get; set; }

        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] ViewsArr { get; set; }

        public int[] ViewsIds { get; set; }

        public string[] FinishArr { get; set; }

        public int[] FinishIds { get; set; }

        public string ShortDescription { get; set; }

        public string DateString { get; set; }
      
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_DemandUnits_Categories_Id { get; set; }

        public int FK_DemandUnits_Transactions_Id { get; set; }

        public int FK_DemandUnits_Regions_FromId { get; set; }

        public int FK_DemandUnits_Regions_ToId { get; set; }

        public int FK_DemandUnits_PaymentMethod_Id { get; set; }


        public int FK_DemandUnits_Users_Sales { get; set; }

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

        public bool IsClosed { get; set; }

        public string BuyerName { get; set; }

        public int FK_DemandUnits_Usage_Id { get; set; }

        public int DateOfBuildFrom { get; set; }

        public int DateOfBuildTo { get; set; }

        public int NoElevatorsFrom { get; set; }

        public int NoElevatorsTo { get; set; }

    }
}
