using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class MixedAvailableClientViewModel
    {
        public int PK_AvailableUnits_Id { get; set; }

        public int FK_AvaliableUnits_Units_UnitId { get; set; }

        [Display(Name = "العميل")]
        [Required(ErrorMessage = "لابد من ملا هذا الحقل")]
        public int FK_AvaliableUnits_Clients_ClientId { get; set; }

        public string ClientName { get; set; }

        public string TransType { get; set; }

        public DateTime CreatedAt { get; set; }

        [Display(Name = "نوع التعامل")]
        [Required(ErrorMessage = "لابد من إختيار نوع التعامل")]
        public int FK_AvaliableUnits_Transaction_TransactionId { get; set; }
     
        [Display(Name = "الملاحظات")]
        public string AvailableNotes { get; set; }

        [Display(Name = "السعر")]
        [Required(ErrorMessage = "لابد من إدخال السعر")]
        public decimal Price { get; set; }

        public decimal AdvancePayment { set; get; }

        [Display(Name = "المتبقي")]
        [Required(ErrorMessage = "لابد من إدخال المبلغ المتبقي")]
        public decimal Remaining { get; set; }
        [Display(Name = "المكسب")]
        [Required(ErrorMessage = "لابد من إدخال المكسب")]
        public decimal Over { get; set; }
        [Display(Name = "سنوات الأقساط")]
        public decimal YearOfInstallment { get; set; }
        [Display(Name = "نظام الأقساط")]
        public byte? BasisOfInstallment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAvailable { get; set; }

        public string SellerName { get; set; }

        [Display(Name = "طريقة الدفــع")]
        [Required(ErrorMessage = "لا بد من إختيار طريقة الدفع")]
        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }

        [Display(Name = "موظف السيلز")]
        [Required(ErrorMessage = "لابد من ملا هذا الحقل")]
        public int FK_AvaliableUnits_Users_SalesId { get; set; }

        public bool IsNegotiable { get; set; }

        public string DateString { get; set; }

        public string ShortDescription { get; set; }

        public UnitViewModel tbl_units { get; set; }

        public SelectList Regions { get; set; }

        public SelectList Finishings { get; set; }

        public SelectList Categories { get; set; }

        public SelectList Transactions { get; set; }
       
        public SelectList Usages { get; set; }

        public SelectList Payments { get; set; }

        public MultiSelectList Accessories { get; set; }

        public SelectList Views { get; set; }

        [Required(ErrorMessage = "لابد من ملا هذا الحقل")]
        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string[] Images { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار استخدام العقار")]
        public int FK_AvailableUnits_Usage_Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار عدد المصاعد")]
        public int NoOfElevators { get; set; }

        //[Required(ErrorMessage = "الرجاء ادخال سنه البناء")]
        public int DateOfBuild { get; set; }


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
        //public string Address { get; set; }

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