using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class MixedAvailableClientDto
    {
        public int PK_AvailableUnits_Id { get; set; }

        public int FK_AvaliableUnits_Units_UnitId { get; set; }

        public int FK_AvaliableUnits_Clients_ClientId { get; set; }

        public string ClientName { get; set; }

        public string TransType { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_AvaliableUnits_Transaction_TransactionId { get; set; }

        public string AvailableNotes { get; set; }
       
        public decimal Price { get; set; }

        public decimal AdvancePayment { set; get; }

        public decimal Remaining { get; set; }
     
        public decimal Over { get; set; }
       
        public decimal YearOfInstallment { get; set; }
       
        public byte? BasisOfInstallment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAvailable { get; set; }

        public string SellerName { get; set; }
      
        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }
       
        public int FK_AvaliableUnits_Users_SalesId { get; set; }

        public bool IsNegotiable { get; set; }

        public string DateString { get; set; }

        public string ShortDescription { get; set; }

        public UnitDto tbl_units { get; set; }   
       
        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] Images { get; set; }

        public int FK_AvailableUnits_Usage_Id { get; set; }

        public int NoOfElevators { get; set; }

        public int DateOfBuild { get; set; }


        //client propereties

        public int PK_Client_Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string IdNumber { get; set; }

        public string Address { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }     

        public string Mobile2 { get; set; }

        public string BestContactHour { get; set; }
    }
}
