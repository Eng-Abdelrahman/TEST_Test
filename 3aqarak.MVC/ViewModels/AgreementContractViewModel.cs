using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class AgreementContractViewModel
    {
        public int PK_AgreementContract_Id { get; set; }

        /// <summary>
        ///  Add today 13/11/2019 by abdelrahman 
        /// </summary>
        public int FK_AgreementcContract_Client_sallerId { get; set; }

        public int FK_AgreementcContract_Client_BuyerId { get; set; }

        public int? Paid { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string SellerAddress { get; set; }

        public string BuyerAddress { get; set; }

        public string Notes { get; set; }

        public DateTime? PostponeDateTime { get; set; }

        public bool IsPostponed { get; set; }


        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        [Required ]
        public string SellerId { get; set; }

        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        [Required(ErrorMessage ="الرجاء إدخال الرقم القومي ")]
        public string BuyerId { get; set; }

        public DateTime DateOfsigningContract { get; set; }

        public int DemandUnits_Id { get; set; }

        public int AvailableUnits_Id { get; set; }

        public string ContractUrl { get; set; }

        public int FK_AvailableCat { get; set; }

        public SelectList Cats { get; set; }

        public string DateTimeString { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }
    }
}