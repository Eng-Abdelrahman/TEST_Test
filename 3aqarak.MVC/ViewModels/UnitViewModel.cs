using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class UnitViewModel /*:IValidatableObject*/
    {
        public int PK_Units_Id { get; set; }

        public int FK_Units_Client_Id { get; set; }

        [Display(Name = "الاطلاله")]
        [Required(ErrorMessage = "الرجاء إدخال الاطلاله")]
        public int FK_Units_Views_Id { get; set; }

        [Display(Name = "المساحة")]
        [Required(ErrorMessage = "الرجاء إدخال المساحه")]
        public int Space { get; set; }

        [Display(Name = "عدد الغرف")]
        [Required(ErrorMessage = "الرجاء إدخال عدد الغرف")]
        public int Rooms { get; set; }

        [Display(Name = "عدد الحمامات")]
        [Required(ErrorMessage = "الرجاء إدخال عدد الحمامات")]
        public int Bathrooms { get; set; }

        [Display(Name = "االطــابق")]
        [Required(ErrorMessage = "الرجاء إدخال الطابق")]
        public int Floor { get; set; }

        [Display(Name = "المنطقــة")]
        [Required(ErrorMessage = "الرجاء إختيار المنطقه")]
        public int FK_Units_Regions_Id { get; set; }

        //[Display(Name = "عنوان الوحدة")]
        //[Required(ErrorMessage = "الرجاء إدخال عنوان الوحدة")]
        //[DataType(DataType.MultilineText, ErrorMessage = "الرجاء إدخال عنوان الوحدة")]
        //public string Address { get; set; }

        [Display(Name = "رقم الشقه")]
        [Required(ErrorMessage = "الرجاء إدخال رقم الشقه")]
        public string FlatNumber { get; set; }

        [Display(Name = "رقم العقار")]
        [Required(ErrorMessage = "الرجاء إدخال رقم العقار")]
        public string ApartmentNumber { get; set; }

        [Display(Name = "رقم المجموعه")]
        [Required(ErrorMessage = "الرجاء إدخال رقم المجموعه")]
        public string GroupNumber { get; set; }


        [Display(Name = "التشطيب")]
        [Required(ErrorMessage = "الرجاء إختيار التشطيب")]
        public int FK_Units_Finishing_Id { get; set; }

        public bool IsFurniture { get; set; }

        public bool IsMarketResearch { get; set; }

        [Display(Name = "وصف الوحدة")]
        [Required(ErrorMessage = "الرجاء إدخال وصف الوحدة")]
        [DataType(DataType.MultilineText)]
        public string Descreption { get; set; }

        [Display(Name = "الفئــه")]
        [Required(ErrorMessage = "الرجاء إختيار الفئة")]
        public int FK_Units_Categories_Id { get; set; }
       
    }
}