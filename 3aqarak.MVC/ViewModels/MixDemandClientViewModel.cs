using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class MixDemandClientViewModel
    {

        //client Propereties

        public int PK_Client_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال إسم العميل")]
        public string Name { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الموبايل")]
        [StringLength(20)]
        //[RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        public string Mobile { get; set; }

        [RegularExpression("^([0-9]{14})$", ErrorMessage = "الرقم القومي لابد ان يتكون من 14 رقم")]
        public string IdNumber { get; set; }

        public string Address { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string Notes { get; set; }

        public string Job { get; set; }

        public bool IsDeleted { get; set; }

        //[RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        [StringLength(20)]
        public string Mobile2 { get; set; }

        [StringLength(50)]
        public string BestContactHour { get; set; }    

        //Demand Propereties

        public int PK_DemandUnits_Id { get; set; }

        public string ClientName { get; set; }

        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار الكماليات المتتاحة")]
        [Display(Name = "الكمــاليات")]
        public string[] AccessoriesArr { get; set; }

        public int[] AccessoriesIds { get; set; }

        public string TransType { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار الإطلالة")]
        [Display(Name = "الاطــلالة")]
        public string[] ViewsArr { get; set; }

        public int[] ViewsIds { get; set; }


        [Required]
        [Display(Name = "التشطيبات")]
        public string[] FinishArr { get; set; }

        public int[] FinishIds { get; set; }


        public string DateString { get; set; }

        public int FK_DemandUnits_Clients_ClientId { get; set; }

        [Display(Name = "التــاريخ")]
        public DateTime CreatedAt { get; set; }     

        [Display(Name = "االملاحظــات")]
        public string DemandNotes { get; set; }

        [Display(Name = "الفــئه")]
        [Required(ErrorMessage = "الرجاء إختيار فئة العقار")]
        public int FK_DemandUnits_Categories_Id { get; set; }

        [Display(Name = "نوع التعامـــل")]
        [Required(ErrorMessage = "الرجاء إختيار نوع التعامل")]
        public int FK_DemandUnits_Transactions_Id { get; set; }

        [Display(Name = "من منطقة:")]
        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_DemandUnits_Regions_FromId { get; set; }

        [Display(Name = "الى منطقة:")]
        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_DemandUnits_Regions_ToId { get; set; }

        [Display(Name = "طــريقة الدفع")]
        [Required(ErrorMessage = "الرجاء إختيار طريقة الدفع")]
        public int FK_DemandUnits_PaymentMethod_Id { get; set; }

        [Display(Name = "اقل سعــر")]
        [Required(ErrorMessage = "الرجاء إدخال أقل سعر")]
        public decimal MinPrice { get; set; }

        [Display(Name = "اعلى سعــر")]
        [Required(ErrorMessage = "الرجاء إدخال أعلى سعر")]
        public decimal MaxPrice { get; set; }

        [Display(Name = "اقل مساحة")]
        [Required(ErrorMessage = "الرجاء إدخال أقل مساحة")]
        public decimal MinSpace { get; set; }

        [Display(Name = "اكبر مساحة")]
        [Required(ErrorMessage = "الرجاء إدخال أكبر مساحة")]
        public decimal MaxSpace { get; set; }

        [Display(Name = "عدد الحمامات من:")]
        [Required(ErrorMessage = "الرجاء إدخال أقل عدد من الحمامات")]
        public int MinBathRooms { get; set; }

        [Display(Name = "عدد الحمامات الى:")]
        [Required(ErrorMessage = "الرجاء إدخال أكبر عدد من الحمامات")]
        public int MaxBathRooms { get; set; }

        [Display(Name = "عدد االغرف من:")]
        [Required(ErrorMessage = "الرجاء إدخال أقل عدد من الغرف")]
        public int MinRooms { get; set; }

        [Display(Name = "عدد االغرف الى:")]
        [Required(ErrorMessage = "الرجاء إدخال أكبر عدد من الغرف")]
        public int MaxRooms { get; set; }

        [Display(Name = "من الطابــق:")]
        [Required(ErrorMessage = "الرجاء إدخال الطابق")]
        public int MinFloor { get; set; }

        [Display(Name = "الى الطابــق:")]
        [Required(ErrorMessage = "الرجاء إدخال الطابق")]
        public int MaxFloor { get; set; }

        [Required]
        [Display(Name = "نوع فرش العقار")]
        public bool IsFurnished { get; set; }

        //public UnitViewModel tbl_units { get; set; }

        public SelectList RegionsFrom { get; set; }

        public SelectList RegionsTo { get; set; }

        public MultiSelectList Finishings { get; set; }

        public SelectList Payments { get; set; }

        public SelectList Usages { get; set; }

        public SelectList Categories { get; set; }

        public SelectList Transactions { get; set; }

        public MultiSelectList Accessories { get; set; }

        public MultiSelectList Views { get; set; }
        // change the name here 
        [Display(Name = "بحث عن موظف السيلز")]
        [Required(ErrorMessage = "لابد من ملا هذا الحقل")]
        public int FK_DemandUnits_Users_Sales { get; set; }

        public string BuyerName { get; set; }

        [Display(Name = "نوع العقار:")]
        [Required(ErrorMessage = "الرجاء تحديد نوع العقار")]
        public int FK_DemandUnits_Usage_Id { get; set; }

        [Display(Name = "تاريخ البناء من:")]
        [Required(ErrorMessage = "الرجاء تاريخ البناء من...")]
        public int DateOfBuildFrom { get; set; }

        //[Display(Name = "تاريخ البناء الى:")]
        //[Required(ErrorMessage = "الرجاء تاريخ البناء الى...")]
        public int DateOfBuildTo { get; set; }

        [Display(Name = "اقل عدد للمصاعد:")]
        [Required(ErrorMessage = "الرجاء تحديد اقل عدد للمصاعد")]
        public int NoElevatorsFrom { get; set; }

        [Display(Name = "اكبر عدد للمصاعد:")]
        [Required(ErrorMessage = "الرجاء تحديد اكبر عدد للمصاعد")]
        public int NoElevatorsTo { get; set; }

    }
}