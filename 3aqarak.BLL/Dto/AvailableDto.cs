﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class AvailableDto
    {
        public int PK_AvailableUnits_Id { get; set; }

        public int FK_AvaliableUnits_Units_UnitId { get; set; }

        public int FK_AvaliableUnits_Clients_ClientId { get; set; }

        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }

        public int FK_Units_Categories_Id { get; set; }

        //public DateTime Date { get; set; }

        public int FK_AvaliableUnits_Transaction_TransactionId { get; set; }

        public int FK_AvaliableUnits_Branches_BranchId { get; set; }

        //public string Title { get; set; }

        public string Notes { get; set; }
      
        public decimal Price { get; set; }
        public decimal AdvancePayment { set; get; }
        public decimal Remaining { get; set; }
        public decimal Over { get; set; }
        public decimal YearOfInstallment { get; set; }
        public byte? BasisOfInstallment { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsNegotiable { get; set; }

        public bool IsClosed { get; set; }

        public string DateString { get; set; }

        public UnitDto tbl_units { get; set; }

        public string ShortDescription { get; set; }

        public string[] AccessoriesArr { get; set; }

        public string[] Images { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string ClientName { get; set; }

        public string SellerName { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_AvaliableUnits_Users_SalesId { get; set; }

        public int FK_AvailableUnits_Usage_Id { get; set; }

        public int NoOfElevators { get; set; }

        public int DateOfBuild { get; set; }












        public string ClientMobile { get; set; }


        public decimal Space { get; set; }


        public int BathRooms { get; set; }

        public int Rooms { get; set; }

        public int Floor { get; set; }



        public bool IsFurnished { get; set; }



        public int FK_AvailableUnits_Categories_Id { get; set; }





        public int FK_AvailableUnits_Transactions_Id { get; set; }


        public int FK_AvaliableUnits_Regions_Id { get; set; }


        public string FlatNumber { get; set; }


        public string ApartmentNumber { get; set; }


        public string GroupNumber { get; set; }


        public int FK_Units_Finishing_Id { get; set; }


        public string Descreption { get; set; }


        public int FK_Units_Views_Id { get; set; }

    }
}
