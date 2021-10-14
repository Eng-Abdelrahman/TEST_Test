using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class ShopAvailableViewModel
    {
        public int PK_ShopAvailable_Id { get; set; }

        public int FK_ShopAvailable_Clients_ClientId { get; set; }

        public string salesName { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public int FK_ShopAvailable_Users_CreatedBy { get; set; }

        public int FK_ShopAvailable_Users_ModidfiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public int FK_ShopAvailable_Categories_Id { get; set; }

        public int FK_ShopAvailable_Transactions_Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المنطقة")]
        public int FK_ShopAvailable_Regions_Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار طرقة الدفع")]
        public int FK_ShopAvailable_PaymentMethod_Id { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال السعر")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال المساحة")]
        public decimal Space { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال عدد الحمامات")]
        public int BathRooms { get; set; }

        public bool IsFurnished { get; set; }

        public bool IsClosed { get; set; }

        public int FK_ShopAvailable_Branches_BranchId { get; set; }

        [Required(ErrorMessage = "لابد من ادخال اسم الموظف")]
        public int FK_ShopAvailable_Users_SalesId { get; set; }

        public bool IsMarketResearch { get; set; }

        public string Address { get; set; }

        public bool IsNegotiable { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال سنة البناء")]
        public int DateOfBuild { get; set; }

        public int ScaleNumber { get; set; }

        public bool IsDivider { get; set; }

        public int FK_ShopAvailable_Usage_Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار نوع الترخيص")]
        public bool Islicense { get; set; }

        // extra props
        public SelectList Regions { get; set; }

        public SelectList Payments { get; set; }

        public SelectList Usages { get; set; }

        public SelectList Categories { get; set; }

        public SelectList Transactions { get; set; }

        public MultiSelectList Accessories { get; set; }

        public MultiSelectList Views { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار الإطلاله")]
        [Display(Name = "الإطلالات")]
        public int FK_ShopAvialable_Views_Id { get; set; }

        public String TransType { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار الكماليات المتاحة")]
        [Display(Name = "الكمــاليات")]
        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] ViewsArr { get; set; }

        public int[] ViewsIds { get; set; }
        //***take care from these 
        public string[] FinishArr { get; set; }
        public string CreatedAtString { get; set; }
        public int[] FinishIds { get; set; }

        public string BuyerName { get; set; }

        public string ScaleName { set; get; }
        public string DividerName { get; set; }
        public string LicenseName { get; set; }
        public string FurnisherName { get; set; }

        //client propereties

        public int PK_Client_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال إسم المستخدم")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الموبايل")]
        [StringLength(20)]
        public string Mobile { get; set; }

        public string IdNumber { get; set; }

        [Display(Name = "العنوان")]
        public string ClientAddress { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }

        [StringLength(20)]
        public string Mobile2 { get; set; }

        [StringLength(50)]
        public string BestContactHour { get; set; }

        public string RegionName { get; set; }

        public string TypeName { get; set; }

        public string[] Images { get; set; }

        public string DateString { get; set; }

        public string ShortDescription { get; set; }

        public string SellerName { get; set; }

        public string  DemandId  {set; get;}
        public string BuyerId { set; get; }

    }
}