using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class ImportDemandViewModel
    {
 
        [Display(Name = "إسم العميل")]
        [Required(ErrorMessage = "الرجاء إدخال إسم العميل")]
        public string ClientName { get; set; }
        [Display(Name = "رقم العميل")]
        public string ClientMobile { get; set; }
        [Display(Name = "سنة البناء من")]
        public int DateOfBuildFrom { get; set; }
        [Display(Name = "سنة البناء إلى")]
        public int DateOfBuildTo { get; set; }
        [Display(Name = "المصاعد من")]
        public int NoElevatorsFrom { get; set; }
        [Display(Name = "المصاعد إلى")]
        public int NoElevatorsTo { get; set; }

        [Display(Name = "السعر من")]
        public decimal MinPrice { get; set; }
        [Display(Name = "السعر إلى")]
        public decimal MaxPrice { get; set; }

        [Display(Name = "المساحه من")]
        public decimal MinSpace { get; set; }
        [Display(Name = "المساحة إلى")]
        public decimal MaxSpace { get; set; }

        [Display(Name = "الحمامات من")]
        public int MinBathRooms { get; set; }

        [Display(Name = "الحمامات إلى")]
        public int MaxBathRooms { get; set; }

        [Display(Name = "الغرف من")]
        public int MinRooms { get; set; }

        [Display(Name = "الغرف إلى")]
        public int MaxRooms { get; set; }

        [Display(Name = "الطابق من")]
        public int MinFloor { get; set; }

        [Display(Name = "الطابق إلى")]
        public int MaxFloor { get; set; }

      
        [Display(Name = "نوع فرش العقار")]
        [Required(ErrorMessage = "الرجاء تحديد نوع الفرش")]
        public bool IsFurnished { get; set; }


        public DateTime CreatedAt { get; set; }


        public int FK_DemandUnits_Categories_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار نوع التعامل")]
        public int FK_DemandUnits_Transactions_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_DemandUnits_Regions_FromId { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_DemandUnits_Regions_ToId { get; set; }

        [Required(ErrorMessage = "الرجاء إختيار طريقة الدفع")]
        public int FK_DemandUnits_PaymentMethod_Id { get; set; }

        [Required(ErrorMessage = "لابد من لإختيار موظف السيلز")]
        public int FK_DemandUnits_Users_Sales { get; set; }
        [Required(ErrorMessage = "الرجاء تحديد نوع العقار")]
        public int FK_DemandUnits_Usage_Id { get; set; }



        public string[] FinishArr { get; set; }
        public string[] AccessoriesArr { get; set; }
        public string[] ViewsArr { get; set; }

    }
}