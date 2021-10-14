using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class SaleHeaderDto
    {        
        public int PK_SalesHeaders_Id { get; set; }

        public int FK_SalesHeaders_Users_EmpId { get; set; }

        public int FK_SalesHeaders_Clients_SellerId { get; set; }

        public int FK_SalesHeaders_Clients_BuyerId { get; set; }      
        
        public decimal TotalAmount { get; set; }

        public DateTime Date { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }

        public int? FK_SalesHeaders_PaymentBasis_Id { get; set; }

        public bool IsDone { get; set; }
       
        public DateTime? DateOfFirstInstall { get; set; }

        public DateTime? DateOfLastInstall { get; set; }

        public DateTime? DateOfNextInstall { get; set; }
        
        public decimal? DefaultInstallValue { get; set; }
        
        public decimal? NextInstallValue { get; set; }
       
        public decimal? ReservDeposit { get; set; }      

        public string ContractUrl { get; set; }

        public string SellerAddress { get; set; }
       
        public string BuyerAddress { get; set; }

        public string SellerId { get; set; }

        public string BuyerId { get; set; }

        public bool IsInstallable { get; set; }

        // add new here ****************
        public decimal PaidAmount { get; set; }

        public decimal DueAmount { get; set; }
    }
}
