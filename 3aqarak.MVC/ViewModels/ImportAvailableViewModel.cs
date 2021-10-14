using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class ImportAvailableViewModel
    {
        [Display(Name = "إسم العميل")]
        [Required(ErrorMessage = "الرجاء إدخال إسم العميل")]
        public string ClientName { get; set; }
       
        [Display(Name = "رقم العميل")]
        public string ClientMobile { get; set; }
      
        [Display(Name = "السعر")]
        public decimal Price { get; set; }    
        
        [Display(Name = "المساحه")]
        public decimal Space { get; set; }
        
        [Display(Name = "عدد الحمامات")]
        public int BathRooms { get; set; }

        [Display(Name = "عدد الغرف")]
        public int Rooms { get; set; }

        [Display(Name = "الطابق")]
        public int Floor { get; set; }

        [Display(Name = "عدد المصاعد")]
        public int NoOfElevators { get; set; }

        [Display(Name = "سنة البناء")]
        public int DateOfBuild { get; set; }
        
        [Display(Name = "رقم الشقه")]
        [Required(ErrorMessage = "الرجاء إدخال رقم الشقه")]
        public string FlatNumber { get; set; }

        [Display(Name = "رقم العقار")]
        [Required(ErrorMessage = "الرجاء إدخال رقم العقار")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "رقم المجموعه")]
        [Required(ErrorMessage = "الرجاء إدخال رقم المجموعه")]
        public string GroupNumber { get; set; }
                       
        [Display(Name = "الاطلاله")]
        [Required(ErrorMessage = "الرجاء إدخال الاطلاله")]
        public int FK_Units_Views_Id { get; set; }
        
        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_AvaliableUnits_Regions_Id { get; set; }

        [Display(Name = "طريقة الدفع")]
        [Required(ErrorMessage = "الرجاء تحديد طريقة الدفع")]
        public int FK_AvailableUnits_PaymentMethod_Id { get; set; }
                        
        [Required(ErrorMessage = "الرجاء إختيار نوع التعامل")]
        public int FK_AvailableUnits_Transactions_Id { get; set; }

        [Display(Name = "نوع إستخدام العقار")]
        [Required(ErrorMessage = "الرجاء إختيار نوع إستخدام العقار")]
        public int FK_AvailableUnits_Usage_Id { get; set; }

        [Display(Name = "التشطيب")]
        [Required(ErrorMessage = "الرجاء إختيار التشطيب")]
        public int FK_Units_Finishing_Id { get; set; }

        [Display(Name = "نوع فرش العقار")]
        [Required(ErrorMessage = "الرجاء تحديد نوع الفرش")]
        public bool IsFurnished { get; set; }

        [Display(Name = "وصف الوحدة")]
        [Required(ErrorMessage = "الرجاء إدخال وصف الوحدة")]
        [DataType(DataType.MultilineText)]
        public string Descreption { get; set; }

        public int FK_AvailableUnits_Categories_Id { get; set; }

        public int FK_AvaliableUnits_Users_SalesId { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
