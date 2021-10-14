using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class MixedVillasAvailableViewModel
    {
        public int PK_VillasAvailables_Id { get; set; }

        public int FK_VillasAvailables_Clients_ClientId { get; set; }

        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] Images { get; set; }
        public string VillNotes { get; set; }

        [Display(Name = "رقم الفيلا")]
        [Required(ErrorMessage = "! لابد من إدخال رقم الفيلا")]
        public string VillaNumber { get; set; }
        [Display(Name = "رقم المجموعه")]
        [Required(ErrorMessage = "! لابد من إدخال رقم المجموعه")]
        public string GroupNumber { get; set; }

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
        public decimal Space { get; set; }

        [Display(Name = "محيط الفيلا")]
        [Required(ErrorMessage = "! لابد من ادخال محيط الفيلا")]
        
        public decimal AreaSpace { get; set; }

        [Display(Name = "الحمامات")]
        [Required(ErrorMessage = "! لابد من ادخال عدد الحمامات")]
        public int BathRooms { get; set; }

        [Display(Name = "الغرف")]
        [Required(ErrorMessage = "! لابد من ادخال عدد الغرف")]
        public int Rooms { get; set; }

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

        [Display(Name = "نوع الإستخدام")]
        [Required(ErrorMessage = "! لابد من إختيار نوع الإستخدام")]
        public int FK_VillasAvailables_Usage_Id { get; set; }

        [Display(Name = "الإطلالة")]
        [Required(ErrorMessage = "! لابد من إختيار الإطلالة")]
        public int FK_VillasAvailables_View_Id { get; set; }

        [Display(Name = "التشطيب")]
        [Required(ErrorMessage = "! لابد من ‘ختيار نوع التشطيب")]
        public int FK_VillasAvailables_Finishings_Id { get; set; }

        [Display(Name = "قابل للتفاوض")]
        public bool IsNegotiable { get; set; }

        [Display(Name = "بحث سوقى")]
        public bool IsMarketResearch { get; set; }

        public string ShortDescription { get; set; }

        public SelectList Regions { get; set; }
        public SelectList Transactions { get; set; }
        public SelectList Payments { get; set; }
        public SelectList Views { get; set; }
        public SelectList Finishings { get; set; }

        public SelectList Categories { get; set; }


        public SelectList Usages { get; set; }


        public MultiSelectList Accessories { get; set; }
        //client propereties

        public int PK_Client_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال إسم المستخدم")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الموبايل")]
        [StringLength(20)]
        public string Mobile { get; set; }

        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string IdNumber { get; set; }

        //[Required(ErrorMessage = "الرجاء إدخال عنوان الوحدة ")]
        public string ClientAddress { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string ClientNotes { get; set; }

        public string Job { get; set; }

        [StringLength(20)]
        public string Mobile2 { get; set; }

        [StringLength(50)]
        public string BestContactHour { get; set; }
    }
}