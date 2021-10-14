using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class FinancialItemsViewModel
    {
        public int PK_Item_Id { get; set; }
        [Required(ErrorMessage ="الرجاء إختيار نوع المعاملة")]
        public bool IsExpenses { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }

        public string Type { get; set; }

        public string DateString { get; set; }
    }
}