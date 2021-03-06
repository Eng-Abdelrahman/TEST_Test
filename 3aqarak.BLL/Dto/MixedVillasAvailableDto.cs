using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class MixedVillasAvailableDto
    {
        public int PK_VillasAvailables_Id { get; set; }

        public int FK_VillasAvailables_Clients_ClientId { get; set; }

        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] Images { get; set; }
        public string VillNotes { get; set; }

        public string VillaNumber { get; set; }

        public string GroupNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public string DateString { get; set; }

        public int FK_VillasAvailables_Users_CreatedBy { get; set; }

        public int FK_VillasAvailables_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_VillasAvailables_Categories_Id { get; set; }

        public int FK_VillasAvailables_Transactions_Id { get; set; }

        public int FK_VillasAvailables_Regions_Id { get; set; }

        public int FK_VillasAvailables_PaymentMethod_Id { get; set; }

        public decimal Price { get; set; }

        public decimal AdvancePayment { set; get; }

        public decimal Remaining { get; set; }

        public decimal Over { get; set; }
        
        public decimal YearOfInstallment { get; set; }
       
        public byte? BasisOfInstallment { get; set; }

        public decimal Space { get; set; }

        public decimal AreaSpace { get; set; }

        public int BathRooms { get; set; }

        public int Rooms { get; set; }


        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_VillasAvailables_Branches_BranchId { get; set; }

        public int FK_VillasAvailables_Users_SalesId { get; set; }

        public int NoOfElevators { get; set; }

        public int DateOfBuild { get; set; }

        public int FK_VillasAvailables_Usage_Id { get; set; }

        public int FK_VillasAvailables_View_Id { get; set; }

        public int FK_VillasAvailables_Finishings_Id { get; set; }

        public bool IsNegotiable { get; set; }

        public bool IsMarketResearch { get; set; }
        public string ShortDescription { get; set; }
        //client propereties

        public int PK_Client_Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string IdNumber { get; set; }

        public string AddressClient { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }

        public string Mobile2 { get; set; }

        public string BestContactHour { get; set; }
    }
}
