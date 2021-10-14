using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _3aqarak.MVC.ViewModels
{
   public class VillsAvailableViewModel
    {

        public int PK_VillasAvailables_Id { get; set; }

        public int FK_VillasAvailables_Clients_ClientId { get; set; }

        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] Images { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DateString { get; set; }

        public int FK_VillasAvailables_Users_CreatedBy { get; set; }

        public int FK_VillasAvailables_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_VillasAvailables_Categories_Id { get; set; }
        [Display(Name = "الأقسام العقارية")]
        [Required(ErrorMessage = "! لابد من إختيار نوع القسم")]
        public int FK_VillasAvailables_Transactions_Id { get; set; }
        [Display(Name = "المنطقه")]
        [Required(ErrorMessage = "! لابد من إختيار المنطقة")]
        public int FK_VillasAvailables_Regions_Id { get; set; }
        [Display(Name = "طريقة الدفع")]
        [Required(ErrorMessage = "! لابد من إختيار طريقة الدفع")]
        public int FK_VillasAvailables_PaymentMethod_Id { get; set; }
        [Display(Name = "السعر")]
        [Required(ErrorMessage = "! لابد من ادخال السعر")]
        //[MinLength(1, ErrorMessage = "السعر لا يمكن ان يكون اقل من 1")]
        public decimal Price { get; set; }

        public decimal AdvancePayment { set; get; }

        [Display(Name = "المتبقي")]
        public decimal Remaining { get; set; }
        [Display(Name = "المكسب")]
        [Required(ErrorMessage = "لابد من إدخال المكسب")]
        public decimal Over { get; set; }
        [Display(Name = "سنوات الأقساط")]
        public decimal YearOfInstallment { get; set; }
        [Display(Name = "نظام الأقساط")]
        public byte? BasisOfInstallment { get; set; }

        [Display(Name = "المساحة")]
        [Required(ErrorMessage = "! لابد من ادخال المساحة")]
        //[MinLength(1, ErrorMessage = "المساحة لا يمكن ان يكون اقل من 1")]
        public decimal Space { get; set; }
        [Display(Name = "محيط الفيلا")]
        [Required(ErrorMessage = "! لابد من ادخال محيط الفيلا")]
        //[MinLength(1, ErrorMessage = "محيط الفيلا لا يمكن ان يكون اقل من 1")]
        public decimal AreaSpace { get; set; }
        [Display(Name = "الحمامات")]
        [Required(ErrorMessage = "! لابد من ادخال عدد الحمامات")]
        public int BathRooms { get; set; }
        [Display(Name = "الغرف")]
        [Required(ErrorMessage = "! لابد من ادخال عدد الغرف")]
        public int Rooms { get; set; }

        [Display(Name = "رقم الفيلا")]
        [Required(ErrorMessage = "! لابد من إدخال رقم الفيلا")]
        public string VillaNumber { get; set; }
        [Display(Name = "رقم المجموعه")]
        [Required(ErrorMessage = "! لابد من إدخال رقم المجموعه")]
        public string GroupNumber { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        [Display(Name = "موظف السيلز")]
        [Required(ErrorMessage = "! لابد من ادخال إسم موظف السيلز")]
        public int FK_VillasAvailables_Users_SalesId { get; set; }
        [Display(Name = "المصاعد")]
        [Required(ErrorMessage = "! لابد من ادخال عدد المصاعد")]
        public int NoOfElevators { get; set; }
        [Display(Name = "تارخ البناء")]
        //[Required(ErrorMessage = "! لابد من ادخال تاريخ الباء")]
        public int DateOfBuild { get; set; }

        public int FK_VillasAvailables_Usage_Id { get; set; }
        [Display(Name = "الإطلالة")]
        [Required(ErrorMessage = "! لابد من إختيار الإطلالة")]
        public int FK_VillasAvailables_View_Id { get; set; }

        public int FK_VillasAvailables_Finishings_Id { get; set; }
        [Display(Name = "قابل للتفاوض")]
        public bool IsNegotiable { get; set; }
        [Display(Name = "بحث سوقى")]
        public bool IsMarketResearch { get; set; }

        public string SellerName { get; set; }

        public string ShortDescription { get; set; }

        public SelectList Usages { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Regions { get; set; }
        public SelectList Transactions { get; set; }
        public SelectList Payments { get; set; }
        public SelectList Views { get; set; }
        public SelectList Finishings { get; set; }
        public MultiSelectList Accessories { get; set; }
        public string RegionName { get; set; }
        public string Type { get; set; }

        public string DemandId { set; get; }
        public string BuyerId { set; get; }
    }
}