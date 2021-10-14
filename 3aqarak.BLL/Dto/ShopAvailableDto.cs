using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class ShopAvailableDto
    {
        public int PK_ShopAvailable_Id { get; set; }

        public int FK_ShopAvailable_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_ShopAvailable_Users_CreatedBy { get; set; }

        public int FK_ShopAvailable_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_ShopAvailable_Categories_Id { get; set; }

        public int FK_ShopAvailable_Transactions_Id { get; set; }

        public int FK_ShopAvailable_Regions_Id { get; set; }

        public int FK_ShopAvailable_PaymentMethod_Id { get; set; }
        
        public decimal Price { get; set; }

        public decimal Space { get; set; }

        public int BathRooms { get; set; }

        public int Rooms { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_ShopAvailable_Branches_BranchId { get; set; }

        public int FK_ShopAvailable_Users_SalesId { get; set; }

        public bool IsMarketResearch { get; set; }

        public string Address { get; set; }

        public bool IsNegotiable { get; set; }

        public int DateOfBuild { get; set; }

        public int ScaleNumber { get; set; }

        public bool IsDivider { get; set; }

        public int FK_ShopAvailable_Usage_Id { get; set; }

        public bool Islicense { get; set; }

        public int FK_ShopAvialable_Views_Id { get; set; }


        public string[] AccessoriesArr { get; set; }

        public string[] Images { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string DateString { get; set; }

        public string ShortDescription { get; set; }

        // client

        public int PK_Client_Id { get; set; }
        
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

        public string SellerName { get; set; }
    }
}
