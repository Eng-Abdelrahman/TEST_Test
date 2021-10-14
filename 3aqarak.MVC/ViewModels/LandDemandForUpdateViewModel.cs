using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class LandDemandForUpdateViewModel
    {
        public int PK_LandsDemands_Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_LandsDemands_Users_CreatedBy { get; set; }

        public int FK_LandsDemands_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string Notes { get; set; }

        public string TransType { get; set; }

        [Required(ErrorMessage = "لابد من إختيار  السعر من")]
        public decimal MinPrice { get; set; }

        [Required(ErrorMessage = "لابد من إختيار  السعر الى")]
        public decimal MaxPrice { get; set; }

        [Required(ErrorMessage = "لابد من إختيار  المساحه من")]
        public decimal MinSpace { get; set; }

        [Required(ErrorMessage = "لابد من إختيار  المساحه الى")]
        public decimal MaxSpace { get; set; }

        public int FK_LandsDemands_Branches_BranchId { get; set; }

        [Required(ErrorMessage = "لابد من إدخال اسم الموظف")]
        public int FK_LandsDemands_Users_SalesId { get; set; }

        public int FK_LandsDemands_Categories_Id { get; set; }

        [Required(ErrorMessage = "لابد من إختيار نوع التعامل")]
        public int FK_LandsDemands_Transactions_Id { get; set; }

        [Required(ErrorMessage = "لابد من إختيار المنطقه من")]
        public int FK_LandsDemands_Regions_FromId { get; set; }

        [Required(ErrorMessage = "لابد من إختيار المنطقه الى")]
        public int FK_LandsDemands_Regions_ToId { get; set; }

        [Required(ErrorMessage = "لابد من إختيار  طريقة الدفع")]
        public int FK_LandsDemands_PaymentMethod_Id { get; set; }

        public int FK_LandsDemands_Clients_ClientId { get; set; }


        public bool IsMatched { get; set; }

        public bool IsClosed { get; set; }

        public string BuyerName { get; set; }

        public string SellerName { get; set; }

        public string TypeString { get; set; }

        public bool Type { get; set; }


        public SelectList RegionsFrom { get; set; }

        public SelectList RegionsTo { get; set; }


        public SelectList Payments { get; set; }

        public SelectList Usages { get; set; }

        public SelectList Categories { get; set; }

        public SelectList Transactions { get; set; }

        public MultiSelectList Accessories { get; set; }

        public MultiSelectList Views { get; set; }

        public string SalseName { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار الإطلالة")]
        [Display(Name = "الاطــلالة")]
        public string[] ViewsArr { get; set; }

        public int[] ViewsIds { get; set; }


    }
}