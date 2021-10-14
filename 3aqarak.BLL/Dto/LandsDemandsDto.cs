using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class LandsDemandsDto
    {
        public int PK_LandsDemands_Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_LandsDemands_Users_CreatedBy { get; set; }

        public int FK_LandsDemands_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_LandsDemands_Clients_ClientId { get; set; }

        public string Notes { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal MinSpace { get; set; }

        public decimal MaxSpace { get; set; }

        public int FK_LandsDemands_Branches_BranchId { get; set; }

        public int FK_LandsDemands_Users_SalesId { get; set; }

        public int FK_LandsDemands_Categories_Id { get; set; }

        public int FK_LandsDemands_Transactions_Id { get; set; }

        public int FK_LandsDemands_Regions_FromId { get; set; }

        public int FK_LandsDemands_Regions_ToId { get; set; }

        public int FK_LandsDemands_PaymentMethod_Id { get; set; }

        public bool IsMatched { get; set; }

        public bool IsClosed { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public bool Type { get; set; }


        public string DateString { get; set; }

        public string ShortDescription { get; set; }

        public int[] ViewsIds { get; set; }
        public string[] ViewsArr { get; set; }
    }
}
