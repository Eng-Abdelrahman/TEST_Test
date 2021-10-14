using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class RentHeaderDto
    {
       
        public int PK_RentAgreements_Id { get; set; }

        public int FK_RentAgreements_Users_EmpId { get; set; }

        public int FK_RentAgreements_Clients_Seller { get; set; }

        public int FK_RentAgreements_Clients_Buyer { get; set; }

        public string ContractUrl { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }

        public DateTime Date { get; set; }

        public DateTime RentalStartDate { get; set; }
     
        public DateTime RentalEndDate { get; set; }

        public int FK_RentAgreementHeader_PaymentBasis_Id { get; set; }
      
        public DateTime DateNxtRent { get; set; }
     
        public decimal ValueOfRental { get; set; }
     
        public string SellerNationality { get; set; }
     
        public string BuyerNationality { get; set; }
      
        public string SellerIdSource { get; set; }
     
        public string BuyerIdSource { get; set; }
        
        public string SellerAddress { get; set; }
       
        public string BuyerAddress { get; set; }
       
        public string SellerIdNumber { get; set; }
      
        public string BuyerIdNumber { get; set; }

      
    }
}
