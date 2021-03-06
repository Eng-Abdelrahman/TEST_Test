using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class LandAvailableForUpdateViewModel
    {
        public int PK_AvailableLands_Id { get; set; }

        [Display(Name = "نوع الارض")]
        [Required(ErrorMessage = "لابد من اختيار نوع الارض")]
        public bool Type { get; set; }

        [Display(Name = "السعر")]
        [Required(ErrorMessage = "لابد من إدخال السعر")]
        public decimal Price { get; set; }

        [Display(Name = "المساحة")]
        [Required(ErrorMessage = "! لابد من ادخال المساحة")]
        public decimal Space { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "! لابد من ادخال العنوان")]
        public string Address { get; set; }

        [Display(Name = "الوصف")]
        [Required(ErrorMessage = "! لابد من ادخال وصف")]
        public string Description { get; set; }

        [Display(Name = "الملاحاظات")]
        public string Notes { get; set; }

        public bool IsClosed { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "قابل للتفاوض")]
        public bool IsNegotiable { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        [Display(Name = "بحث سوقى")]
        public bool IsMarketSearch { get; set; }

        [Display(Name = "طريقة الدفع")]
        [Required(ErrorMessage = "لابد من اختيار طريقة الدفع")]
        public int FK_AvailableLands_PaymentMethod_Id { get; set; }

        public int FK_AvaliableLands_Branches_BranchId { get; set; }

        [Display(Name = "الاقسام العقارية")]
        [Required(ErrorMessage = "لابد من اختيار نوع القسم")]
        public int FK_AvaliableLands_Transactions_TransactionId { get; set; }

        public int FK_AvaliableLands_Users_ModifiedBy { get; set; }

        public int FK_AvaliableLands_Users_CreatedBy { get; set; }

        public int FK_AvaliableLands_Clients_ClientId { get; set; }

        [Display(Name = "مــوظف السيــليز")]
        [Required(ErrorMessage = "لابد من إدخال موظف السيليز")]
        public int FK_AvaliableLands_Users_SalesId { get; set; }

        [Display(Name = "الاطلاله")]
        [Required(ErrorMessage = "لابد من اختيار الاطلالة")]
        public int FK_AvailableLands_Views_ViewId { get; set; }

        public int FK_AvailableLands_Categories_CategoryId { get; set; }

        [Display(Name = "المنطقة")]
        [Required(ErrorMessage = "لابد من اختيار المنطقة")]
        public int FK_AvailabeLands_Regions_RegionId { get; set; }

        public string BuyrName { get; set; }

        public string SellerName { get; set; }
        public string TransType { get; set; }

        public SelectList Regions { get; set; }
        public SelectList Transactions { get; set; }
        public SelectList Payments { get; set; }
        public SelectList Views { get; set; }
    }
}