using _3aqarak.MVC.Areas.Dashboard.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class ClientViewModel
    {

        public int PK_Client_Id { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال إسم المستخدم")]
        public string Name { get; set; }

        //[RegularExpression("(0)[1-9]{9}", ErrorMessage = "رقم الهاتف غير صحيح!")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الموبايل")]
        //[RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        [StringLength(20)]
        public string Mobile { get; set; }

       
        [RegularExpression("^([0-9]{14})$", ErrorMessage ="الرقم القومي لابد ان يتكون من 14 رقم")]
        public string IdNumber { get; set; }
        
        public string Address { get; set; }

        public string IdUrlFront { get; set; }

        public string IdUrlBack { get; set; }

        public string Notes { get; set; }
      
        public string Job { get; set; }

        public bool IsDeleted { get; set; }

        //[RegularExpression("(01)[0-9]{9}", ErrorMessage = "رقم الموبايل غير صحيح!")]
        //[CompareMobile(nameof(Mobile))]
        [StringLength(20)]
        public string Mobile2 { get; set; }
      
        [StringLength(50)]
        public string BestContactHour { get; set; }     

     
    }
}