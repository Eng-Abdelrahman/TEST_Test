using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class RentContractViewModel
    {       
        public int PK_RentAgreements_Id { get; set; }

        public int FK_RentAgreements_Users_EmpId { get; set; }

        public int FK_RentAgreements_Clients_Seller { get; set; }

        public int FK_RentAgreements_Clients_Buyer { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public string EmpName { get; set; }

        public string ContractUrl { get; set; }

        public DateTime Date { get; set; }

        public string DateString { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public int AvailableCat { get; set; }

        public int DemandCat { get; set; }
        
        [Required(ErrorMessage ="الرجاء إدخال تاريخ بداية الإيجار")]
        public DateTime RentalStartDate { get; set; }

        [Required(ErrorMessage ="الرجاء إدخال تاريخ نهاية الإيجار")]
        public DateTime RentalEndDate { get; set; }

        public int FK_RentAgreementHeader_PaymentBasis_Id { get; set; }
        [Required(ErrorMessage ="الرجاء إدخال تاريخ الإيجار القادم")]
        public DateTime DateNxtRent { get; set; }

        [Required]
        public decimal ValueOfRental { get; set; }      

        [Required]
        [StringLength(50)]
        public string SellerNationality { get; set; }

        [Required]
        [StringLength(50)]
        public string BuyerNationality { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerIdSource { get; set; }

        [Required]
        [StringLength(50)]
        public string BuyerIdSource { get; set; }

        [Required]
        public string SellerAddress { get; set; }

        [Required]
        public string BuyerAddress { get; set; }

        [Required]
        [StringLength(14)]
        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string SellerIdNumber { get; set; }

        [Required]
        [StringLength(14)]
        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string BuyerIdNumber { get; set; }

        [Required]
        [AllowHtml]
        public string DetailContent { get; set; }
        
        public SelectList Basis { get; set; }

        public SelectList Cats { get; set; }

        public string TypeOfContract { get; set; }
    }
}