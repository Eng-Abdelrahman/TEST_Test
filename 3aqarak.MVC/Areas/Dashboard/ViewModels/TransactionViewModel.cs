using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class TransactionViewModel
    {       
        public int PK_Transactions_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TransType { get; set; }

        public int TransCode { get; set; }
    }
}