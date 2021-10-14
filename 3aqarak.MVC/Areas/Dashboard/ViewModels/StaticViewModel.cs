using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class StaticViewModel
    {

        public int PK_StatContract_Id { get; set; }

        [Required(ErrorMessage ="لابد من ملأ هذا الحقل")]
        [Display(Name ="الفئــــه")]
        public int FK_StaticContract_Categories_CatId { get; set; }

        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        [Display(Name = "التعامـــل")]
        public int FK_StaticContract_Transaction_Transid { get; set; }

        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        [AllowHtml]
        public string ContactContent { get; set; }

         public SelectList Cats { get; set; }

        public SelectList Trans { get; set; }
    }
}