using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class RegionViewModel
    {
        public int PK_Regions_ID { get; set; }

        [Required]
        [Display(Name = "اسم المنطقة")]
        public string Region { get; set; }

        [Display(Name ="ترتيب المنطقة")]
        [RegularExpression(@"^[1-9]\d*$",ErrorMessage ="لايسمح برقم منطقة 0")]
        public int RegCode { get; set; }
    }
}