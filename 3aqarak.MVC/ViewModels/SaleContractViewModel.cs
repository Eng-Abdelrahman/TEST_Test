using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class SaleContractViewModel
    {
      
        public int PK_SalesHeaders_Id { get; set; }

        public int FK_SalesHeaders_Users_EmpId { get; set; }

        public int FK_SalesHeaders_Clients_SellerId { get; set; }

        public int FK_SalesHeaders_Clients_BuyerId { get; set; }

        public decimal TotalAmount { get; set; }

        public string Installement { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }

        public int? FK_SalesHeaders_PaymentBasis_Id { get; set; }

        public bool IsDone { get; set; }

        public DateTime Date { get; set; }

        public string DateString { get; set; }

        public DateTime? DateOfFirstInstall { get; set; }
        
        public DateTime? DateOfNextInstall { get; set; }

        public DateTime? DateOfLastInstall { get; set; }

        public decimal? DefaultInstallValue { get; set; }
       
        public decimal? NextInstallValue { get; set; }
     
        public decimal? ReservDeposit { get; set; }   

        public string ContractUrl { get; set; }

        [Required]
        public string SellerAddress { get; set; }

        [Required]
        public string BuyerAddress { get; set; }

        [Required]
        [StringLength(14)]     
        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string SellerId { get; set; }

        [Required]
        [StringLength(14)]     
        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string BuyerId { get; set; }

        public bool IsInstallable { get; set; }


        [Required]
        [AllowHtml]
        public string DetailContent { get; set; }

        public SelectList Basis { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public string EmpName { get; set; }
        // add newe here *************
        public decimal PaidAmount { get; set; }

        public decimal DueAmount { get; set; }
        public SelectList Cats { get; set; }

        public string TypeOfContract { get; set; }
    }
}