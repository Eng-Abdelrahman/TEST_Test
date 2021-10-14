using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class ContractArchiveViewModel
    {
        public int PK_ContractArchives_Id { get; set; }

        
        public string ImageURL { get; set; }

        
        //public string ImageURLFront { get; set; }

        [Required]
        public string ContractID { get; set; }

        
    }
}