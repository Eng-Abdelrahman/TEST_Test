using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class AvailableLandsDto
    {
        public int PK_AvailableLands_Id { get; set; }

        public bool Type { get; set; }

        public decimal Price { get; set; }

        public decimal Space { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        public bool IsClosed { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsNegotiable { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsMarketSearch { get; set; }

        public int FK_AvailableLands_PaymentMethod_Id { get; set; }

        public int FK_AvaliableLands_Branches_BranchId { get; set; }

        public int FK_AvaliableLands_Transactions_TransactionId { get; set; }

        public int FK_AvaliableLands_Users_ModifiedBy { get; set; }

        public int FK_AvaliableLands_Users_CreatedBy { get; set; }

        public int FK_AvaliableLands_Clients_ClientId { get; set; }

        public int FK_AvaliableLands_Users_SalesId { get; set; }

        public int FK_AvailableLands_Views_ViewId { get; set; }

        public int FK_AvailableLands_Categories_CategoryId { get; set; }

        public int FK_AvailabeLands_Regions_RegionId { get; set; }

        public string SellerName { get; set; }

        public string[] Images { get; set; }

        //client propereties

        public string Name { get; set; }
        
        public string Phone { get; set; }
        
        public string Mobile { get; set; }
        
        public string IdNumber { get; set; }
        
        public string ClientAddress { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }
        
        public string Mobile2 { get; set; }
        
        public string BestContactHour { get; set; }

        public string DateString { get; set; }

        public string ShortDescription { get; set; }
    }
}
