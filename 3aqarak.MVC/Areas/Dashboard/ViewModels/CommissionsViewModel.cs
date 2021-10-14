using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class CommissionsViewModel
    {
        public int PK_Commissions_Id { get; set; }

        [Display(Name = "نسبة التلي سيلز")]
        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        public decimal TelesalesComission { get; set; }

        [Display(Name = "نسبة  السيلز")]
        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        public decimal SalesComission { get; set; }

        [Display(Name = "نسبة  مدير الفرع")]
        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        public decimal MgrCommission { get; set; }

        [Display(Name = "نوع المعاملة التجارية")]
        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        public int FK_Commissions_Transactions_Id { get; set; }

        [Display(Name = "فئه العقار")]
        [Required(ErrorMessage = "لابد من ملأ هذا الحقل")]
        public int FK_Commissions_Categories_Id { get; set; }

        public string Name { get; set; }

        public SelectList  Categories { get; set; }

        public SelectList Transactions { get; set; }

    }
}