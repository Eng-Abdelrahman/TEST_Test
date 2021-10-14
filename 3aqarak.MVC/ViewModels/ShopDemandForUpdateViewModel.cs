using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class ShopDemandForUpdateViewModel
    {
        public int PK_ShopDemands_Id { get; set; }

        public int FK_ShopDemands_Clients_ClientId { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار الكماليات المتاحة")]
        [Display(Name = "الكمــاليات")]
        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار الإطلاله")]
        [Display(Name = "الإطلالات")]
        public string[] ViewsArr { get; set; }

        public int[] ViewsIds { get; set; }
        //***take care from these 
        public string[] FinishArr { get; set; }
        public string ShortDescription { get; set; }
        public string CreatedAtString { get; set; }
        public int[] FinishIds { get; set; }
        //*** look up to these proparty ^^^^^
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_ShopDemands_Users_CreatedBy { get; set; }

        public int FK_ShopDemands_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_ShopDemands_Categories_Id { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار نوع القسم")]
        [Display(Name = "الأقسام العقارية")]
        public int FK_ShopDemands_Transactions_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار المنطقة من")]
        [Display(Name = "المناطق")]
        public int FK_ShopDemands_Regions_FromId { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار المنطقة إلى")]
        [Display(Name = "المناطق")]
        public int FK_ShopDemands_Regions_ToId { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار طريقة الدفع")]
        [Display(Name = "طرق الدفع")]
        public int FK_ShopDemands_PaymentMethod_Id { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أقل قيمة للسعر")]
        [Display(Name = "أقل سعر")]
        public decimal MinPrice { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أعلى قيمة للسعر")]
        [Display(Name = "أعلى سعر")]
        public decimal MaxPrice { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أقل قيمة للمساحة")]
        [Display(Name = "أقل مساحة")]
        public decimal MinSpace { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أعلى قيمة للمساحة")]
        [Display(Name = "أكبر مساحة")]
        public decimal MaxSpace { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أقل قيمة للحمامات")]
        [Display(Name = " الحمامات")]
        public int MinBathRooms { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أعلى قيمة للحمامات")]
        [Display(Name = " الحمامات")]
        public int MaxBathRooms { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار نوع المحل")]
        [Display(Name = " نوع المحل")]
        public int ScaleNumber { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار نوع التقسيم")]
        [Display(Name = " نوع التقسيم")]
        public bool IsDivider { get; set; }

        public bool IsClosed { get; set; }

        public int FK_ShopDemands_Branches_BranchId { get; set; }

        public int FK_ShopDemands_Users_SalesId { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أقدم تاريخ بناء")]
        [Display(Name = " تاريخ البناء")]
        public int DateOfBuildFrom { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال أحدث تاريخ بناء")]
        [Display(Name = " تاريخ البناء")]
        public int DateOfBuildTo { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار إستخدام العقار")]
        [Display(Name = " إستخدام العقار")]
        public int FK_ShopDemands_Usage_Id { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار نوع الترخيص")]
        [Display(Name = " نوع الترخيص")]
        public bool Islicense { get; set; }
        [Required(ErrorMessage = "الرجاء إختيار نوع التجهيز")]
        [Display(Name = " نوع التجهيز")]
        public bool IsFurnisher { get; set; }

        public string ScaleName { set; get; }
        public string DividerName { get; set; }
        public string LicenseName { get; set; }
        public string Furnisher { get; set; }

        //**********************

        public string DateString { get; set; }

        public string BuyerName { get; set; }

        public SelectList RegionsFrom { get; set; }

        public SelectList RegionsTo { get; set; }


        public SelectList Payments { get; set; }

        public SelectList Usages { get; set; }

        public SelectList Categories { get; set; }

        public SelectList Transactions { get; set; }

        public MultiSelectList Accessories { get; set; }

        public MultiSelectList Views { get; set; }

        public String TransType { get; set; }


        public int PK_Client_Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }


        public string Mobile { get; set; }

        public string IdNumber { get; set; }

        public string ClientAddress { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }


        public string Mobile2 { get; set; }

        public string BestContactHour { get; set; }

        public string RegionNameFrom { set; get; }

        public string RegionNameTo { get; set; }

        public string AvailableId { set; get; }

        public string selerId { set; get; }
    }
}