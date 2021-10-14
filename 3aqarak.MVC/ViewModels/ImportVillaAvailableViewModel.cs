using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class ImportVillaAvailableViewModel
    {

        public string ClientName { get; set; }

       
        public string ClientMobile { get; set; }

        
        public decimal Price { get; set; }

       
        public decimal Space { get; set; }

       
        public int BathRooms { get; set; }

        
        public int Rooms { get; set; } 
        
        
        public int AreaSpace { get; set; }

       
        public int NoOfElevators { get; set; }

        
        public string VillaNumber { get; set; }

            
        public string GroupNumber { get; set; }


        public int DateOfBuild { get; set; }


        public string Descreption { get; set; }


        public int FK_Units_Views_Id { get; set; }

        
        public int FK_AvaliableUnits_Regions_Id { get; set; }

       
        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }

        
        public int FK_AvailableUnits_Transactions_Id { get; set; }

        
        public int FK_AvailableUnits_Usage_Id { get; set; }

        
        public int FK_Units_Finishing_Id { get; set; }

        
        public bool IsFurnished { get; set; }


        public int FK_VillasAvailables_Categories_Id { get; set; }

      
        public int FK_VillasAvailables_Users_SalesId { get; set; }


        public DateTime CreatedAt { get; set; }

    }
}